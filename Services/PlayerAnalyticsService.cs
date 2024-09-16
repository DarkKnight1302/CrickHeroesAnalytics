using CricHeroesAnalytics.Constants;
using CricHeroesAnalytics.Entities;
using CricHeroesAnalytics.Models.ScoreCardModels;
using CricHeroesAnalytics.Repositories;
using CricHeroesAnalytics.Services.Interfaces;
using Microsoft.AspNetCore.Routing.Constraints;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CricHeroesAnalytics.Services
{
    public class PlayerAnalyticsService : IPlayerAnalyticsService
    {
        private const long Cult100TeamId = 5455774;
        private readonly ILogger _logger;
        private readonly IPlayerRepository _playerRepository;
        public PlayerAnalyticsService(IPlayerRepository playerRepository, ILogger<PlayerAnalyticsService> logger) 
        {
            _playerRepository = playerRepository;
            this._logger = logger;
        }
        public async Task UpdatePlayerStatsForMatch(string matchId, List<Scorecard> Scorecard)
        {
            Scorecard s1 = Scorecard[0];
            Scorecard s2 = Scorecard[1];

            if (s1.TeamId == Cult100TeamId)
            {
                await UpdateBatting(matchId, s1.Batting);
                await UpdateBowling(matchId, s2.Bowling);
            } else
            {
                await UpdateBatting(matchId, s2.Batting);
                await UpdateBowling(matchId, s1.Bowling);
            }
        }

        private async Task UpdateBatting(string matchId, List<Batting> batting)
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
                int outCount = GlobalConstants.NotOutList.Contains(battingStats.HowToOut) ? 1 : 0;
                player.GotOutCount += outCount;
                PlayerRunsPerMatch playerRunsPerMatch = new PlayerRunsPerMatch();
                playerRunsPerMatch.MatchId = matchId;
                playerRunsPerMatch.Runs = battingStats.Runs;
                playerRunsPerMatch.BallsPlayed = battingStats.Balls;
                player.PlayerRunMatchMap[matchId] = playerRunsPerMatch;
                UpdateTotalRuns(player);
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
            }
        }

        private async Task UpdateBowling(string matchId, List<Bowling> bowling)
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
    }
}
