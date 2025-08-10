using CricHeroesAnalytics.Models;
using CricHeroesAnalytics.Models.ScoreCardModels;

namespace CricHeroesAnalytics.Services.Interfaces
{
    public interface ICricHeroesApiClient
    {
        public Task<List<MatchData>> GetMatches();

        public Task<ScoreCardResponse> GetScoreCard(MatchData matchData);

        public void FetchBuildUsingSelenium();

        public void UpdateBuildId(string buildId);

        public Task<string?> GetProfilePictureUrlAsync(string playerId);

        public void ResetCache();
    }
}
