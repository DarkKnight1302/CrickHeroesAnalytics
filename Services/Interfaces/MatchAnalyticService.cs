
using CricHeroesAnalytics.Models;

namespace CricHeroesAnalytics.Services.Interfaces
{
    public class MatchAnalyticService : IMatchAnalyticService
    {
        private readonly ILogger _logger;
        private readonly ICricHeroesApiClient cricHeroesApiClient;

        public MatchAnalyticService(ILogger<IMatchAnalyticService> logger, ICricHeroesApiClient cricHeroesApiClient) 
        {
            _logger = logger;
            this.cricHeroesApiClient = cricHeroesApiClient;
        }

        public async Task UpdateLatestMatchData()
        {
            List<MatchData> matchData = await this.cricHeroesApiClient.GetMatches();
            if (matchData == null)
            {
                _logger.LogInformation("No matches found");
                return;
            }
            matchData = FilterValidMatches(matchData);
            matchData.Sort(Compare);
            // Check in db.
            await this.cricHeroesApiClient.GetScoreCard(matchData[0]);
        }

        private List<MatchData> FilterValidMatches(List<MatchData> matchData) 
        {
            List<MatchData> validMatches = new List<MatchData>();
            foreach(var match in  matchData)
            {
                if (match.MatchResult.Equals("Resulted") && match.Status.Equals("past"))
                {
                    validMatches.Add(match);
                }
            }
            return validMatches;
        }

        public int Compare(MatchData a, MatchData b) 
        {
            return b.MatchStartTime.CompareTo(a.MatchStartTime);
        }
    }

}
