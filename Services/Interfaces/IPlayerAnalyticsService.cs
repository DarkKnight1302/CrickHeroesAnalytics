using CricHeroesAnalytics.Models;
using CricHeroesAnalytics.Models.ScoreCardModels;

namespace CricHeroesAnalytics.Services.Interfaces
{
    public interface IPlayerAnalyticsService
    {
        public Task UpdatePlayerStatsForMatch(string matchId, List<Scorecard> Scorecard, DateTime matchStartTime);

        public List<Entities.Player> GetAllPlayers();

        public Task<List<Entities.Player>> GetAllPlayersAsync();

        public Task<List<Entities.Player>> GetAllRounderLeaderboard();

        public Task<List<Entities.Player>> GetSuperStrikerLeaderboard();

        public List<Entities.Player> GetBattersByRank();

        public List<Entities.Player> GetBowlersByRank();

        public Task<CapHolders> GetCapHoldersAsync();
    }
}
