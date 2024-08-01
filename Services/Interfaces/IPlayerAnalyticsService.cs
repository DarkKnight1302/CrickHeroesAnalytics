using CricHeroesAnalytics.Entities;
using CricHeroesAnalytics.Models;
using CricHeroesAnalytics.Models.ScoreCardModels;

namespace CricHeroesAnalytics.Services.Interfaces
{
    public interface IPlayerAnalyticsService
    {
        public Task UpdatePlayerStatsForMatch(string matchId, List<Scorecard> Scorecard);

        public List<Entities.Player> GetAllPlayers();
    }
}
