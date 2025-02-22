using CricHeroesAnalytics.Constants;
using CricHeroesAnalytics.Entities;
using CricHeroesAnalytics.Models.ScoreCardModels;
using CricHeroesAnalytics.Repositories;
using CricHeroesAnalytics.Services.Interfaces;

namespace CricHeroesAnalytics.Services
{
    public class PlayerAnalyticsService : IPlayerAnalyticsService
    {
        private const long Cult100TeamId = 5455774;
        private readonly ILogger _logger;
        private readonly IPlayerRepository _playerRepository;
        private readonly IPlayerRatingService playerRatingService;

        public PlayerAnalyticsService(IPlayerRepository playerRepository, ILogger<PlayerAnalyticsService> logger, IPlayerRatingService playerRatingService) 
        {
            _playerRepository = playerRepository;
            this._logger = logger;
            this.playerRatingService = playerRatingService;
        }
        public async Task UpdatePlayerStatsForMatch(string matchId, List<Scorecard> Scorecard, DateTime matchStartTime)
        {
            Scorecard s1 = Scorecard[0];
            Scorecard s2 = Scorecard[1];

            if (s1.TeamId == Cult100TeamId)
            {
                await UpdateBatting(matchId, s1.Batting, matchStartTime);
                await UpdateBowling(matchId, s2.Bowling, matchStartTime);
            } else
            {
                await UpdateBatting(matchId, s2.Batting, matchStartTime);
                await UpdateBowling(matchId, s1.Bowling, matchStartTime);
            }
        }

        private async Task UpdateBatting(string matchId, List<Batting> batting, DateTime matchStarted)
        {
            foreach(Batting battingStats in batting)
            {
                string playerIdString = battingStats.PlayerId.ToString();
                Entities.Player player = await this._playerRepository.GetPlayer(playerIdString);
                if (player == null)
                {
                    player = new Entities.Player();
                    player.Id = playerIdString;
                    player.Uid = playerIdString;
                    player.Name = battingStats.Name;
                }
                if (player.PlayerRunMatchMap.ContainsKey(matchId))
                {
                    continue;
                }
                int outCount = GlobalConstants.NotOutList.Contains(battingStats.HowToOut) ? 0 : 1;
                player.GotOutCount += outCount;
                PlayerRunsPerMatch playerRunsPerMatch = new PlayerRunsPerMatch();
                playerRunsPerMatch.MatchId = matchId;
                playerRunsPerMatch.Runs = battingStats.Runs;
                playerRunsPerMatch.BallsPlayed = battingStats.Balls;
                playerRunsPerMatch.WasNotOut = GlobalConstants.NotOutList.Contains(battingStats.HowToOut);
                player.PlayerRunMatchMap[matchId] = playerRunsPerMatch;
                UpdateTotalRuns(player);
                player.LastMatchUpdated = matchStarted;
                await this._playerRepository.CreateOrUpdatePlayer(player);
            }
        }

        private void UpdateTotalRuns(Entities.Player player)
        {
            int totalRuns = 0;
            int totalBalls = 0;
            foreach(var kv in player.PlayerRunMatchMap)
            {
                totalRuns += kv.Value.Runs;
                totalBalls += kv.Value.BallsPlayed;
            }
            player.TotalRuns = totalRuns;
            player.BallsPlayed = totalBalls;
            if (totalBalls > 0)
            {
                player.StrikeRate = (totalRuns * 100) / totalBalls;
            }
            player.MatchesPlayed = player.PlayerRunMatchMap.Count;
        }

        private void UpdateTotalWickets(Entities.Player player)
        {
            int totalWickets = 0;
            int runsGiven = 0;
            int overs = 0;
            int extraBalls = 0;
            foreach (var kv in player.PlayerWicketsMatchMap)
            {
                totalWickets += kv.Value.Wickets;
                runsGiven += kv.Value.RunsGiven;
                overs += (int)kv.Value.Overs;
                extraBalls += (int)((kv.Value.Overs * 10) % 10);
            }
            player.TotalWickets = totalWickets;
            int totalBalls = overs * 6 + extraBalls;
            double totalOvers = ((double)totalBalls / 6D);
            player.OversBowled = totalOvers;
            player.RunsGiven = runsGiven;
            if (totalOvers > 0)
            {
                player.BowlingEconomy = Math.Round(((double)runsGiven / (double)totalOvers), 1);
                if (player.TotalWickets > 0)
                {
                    double bowlingAverage = ((double)player.RunsGiven / (double)player.TotalWickets);
                    player.BowlingAverage = Math.Round(bowlingAverage, 1);
                }
            }
        }

        private async Task UpdateBowling(string matchId, List<Bowling> bowling, DateTime matchStartTime)
        {
            foreach (Bowling bowlingStats in bowling)
            {
                string playerIdString = bowlingStats.PlayerId.ToString();
                Entities.Player player = await this._playerRepository.GetPlayer(playerIdString);
                if (player == null)
                {
                    player = new Entities.Player();
                    player.Id = playerIdString;
                    player.Uid = playerIdString;
                    player.Name = bowlingStats.Name;
                }
                if (player.PlayerWicketsMatchMap.ContainsKey(matchId))
                {
                    continue;
                }
                PlayerWicketsPerMatch playerWicketsPerMatch = new PlayerWicketsPerMatch();
                playerWicketsPerMatch.MatchId = matchId;
                playerWicketsPerMatch.Wickets = bowlingStats.Wickets;
                playerWicketsPerMatch.RunsGiven = bowlingStats.Runs;
                playerWicketsPerMatch.Overs = bowlingStats.Overs;
                player.PlayerWicketsMatchMap[matchId] = playerWicketsPerMatch;
                UpdateTotalWickets(player);
                player.LastMatchUpdated = matchStartTime;
                await this._playerRepository.CreateOrUpdatePlayer(player);
            }
        }

        public List<Entities.Player> GetAllPlayers()
        {
            return this._playerRepository.GetAllPlayers();
        }

        public async Task<List<Entities.Player>> GetAllPlayersAsync()
        {
            return await this._playerRepository.GetAllPlayersAsync();
        }

        public List<Entities.Player> GetAllRounderLeaderboard()
        {
            var allPlayers = GetAllPlayers();
            allPlayers = GetAllRounderApplicablePlayers(allPlayers);
            Dictionary<string, double> battingRating = new Dictionary<string, double>();
            Dictionary<string, double> bowlingRating = new Dictionary<string, double>();
            Dictionary<string, double> allRoundRating = new Dictionary<string, double>();

            // Step 1: Calculate ratings and find min/max values
            foreach (var player in allPlayers)
            {
                double battingRatingPlayer = this.playerRatingService.GetBattingRating(player);
                double bowlingRatingPlayer = this.playerRatingService.GetBowlingRating(player);

                battingRating.TryAdd(player.Id, battingRatingPlayer);
                bowlingRating.TryAdd(player.Id, bowlingRatingPlayer);
            }
            List<Entities.Player> battingRanked = new List<Entities.Player>(allPlayers);
            List<Entities.Player> bowlingRanked = new List<Entities.Player>(allPlayers);

            battingRanked.Sort((a, b) =>
            {
                double a1 = battingRating[a.Id];
                double b1 = battingRating[b.Id];

                return b1.CompareTo(a1);
            });
            bowlingRanked.Sort((a, b) =>
            {
                double a1 = bowlingRating[a.Id];
                double b1 = bowlingRating[b.Id];

                return b1.CompareTo(a1);
            });

            int v = 1;
            for (int i=battingRanked.Count-1; i>=0; i--)
            {
                var player = battingRanked[i];
                allRoundRating[player.Id] = v;
                this._logger.LogInformation($"Batting weight for player {player.Name} - {v}");
                v++;
            }
            v = 1;
            for (int i = bowlingRanked.Count - 1; i >= 0; i--)
            {
                var player = bowlingRanked[i];
                allRoundRating[player.Id] += v;
                this._logger.LogInformation($"Bowling weight for player {player.Name} - {v}");
                v++;
            }

            // Step 3: Sort players by total rating
            allPlayers.Sort((a, b) =>
            {
                double ar = allRoundRating[a.Id];
                double br = allRoundRating[b.Id];
                if (ar == br)
                {
                    double a1 = battingRating[a.Id];
                    double b1 = battingRating[b.Id];

                    return b1.CompareTo(a1);
                }
                return br.CompareTo(ar);
            });

            return allPlayers;

        }

        private List<Entities.Player> GetAllRounderApplicablePlayers(List<Entities.Player> players)
        {
            List<Entities.Player> allRounders = new List<Entities.Player>();
            foreach(var player in players)
            {
                if (player.MatchesPlayed > 5 && player.OversBowled >= 10 && player.BattingAverage >= 10 && player.LastMatchUpdated > DateTime.Now.AddDays(-30))
                {
                    allRounders.Add(player);
                }
            }
            return allRounders;
        }
    }
}
