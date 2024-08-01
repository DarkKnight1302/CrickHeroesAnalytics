using CricHeroesAnalytics.Models;
using CricHeroesAnalytics.Models.ScoreCardModels;

namespace CricHeroesAnalytics.Services.Interfaces
{
    public interface ICricHeroesApiClient
    {
        public Task<List<MatchData>> GetMatches();

        public Task<ScoreCardResponse> GetScoreCard(MatchData matchData);
    }
}
