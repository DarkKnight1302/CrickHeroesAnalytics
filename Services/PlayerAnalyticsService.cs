using CricHeroesAnalytics.Entities;
using CricHeroesAnalytics.Models;
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
                PlayerRunsPerMatch playerRunsPerMatch = new PlayerRunsPerMatch();
                playerRunsPerMatch.MatchId = matchId;
                playerRunsPerMatch.Runs = battingStats.Runs;
                player.PlayerRunMatchMap[matchId] = playerRunsPerMatch;
                UpdateTotalRuns(player);
                await this._playerRepository.CreateOrUpdatePlayer(player);
            }
        }

        private void UpdateTotalRuns(Entities.Player player)
        {
            int totalRuns = 0;
            foreach(var kv in player.PlayerRunMatchMap)
            {
                totalRuns += kv.Value.Runs;
            }
            player.TotalRuns = totalRuns;
            player.MatchesPlayed = player.PlayerRunMatchMap.Count;
        }

        private void UpdateTotalWickets(Entities.Player player)
        {
            int totalWickets = 0;
            foreach (var kv in player.PlayerWicketsMatchMap)
            {
                totalWickets += kv.Value.Wickets;
            }
            player.TotalWickets = totalWickets;
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
                player.PlayerWicketsMatchMap[matchId] = playerWicketsPerMatch;
                UpdateTotalWickets(player);
                await this._playerRepository.CreateOrUpdatePlayer(player);
            }
        }
    }
}
