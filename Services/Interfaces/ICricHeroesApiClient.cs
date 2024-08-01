using CricHeroesAnalytics.Models;

namespace CricHeroesAnalytics.Services.Interfaces
{
    public interface ICricHeroesApiClient
    {
        public Task<List<MatchData>> GetMatches();

        public Task<object> GetScoreCard(MatchData matchData);
    }
}
