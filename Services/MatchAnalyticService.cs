using CricHeroesAnalytics.Entities;
using CricHeroesAnalytics.Models;
using CricHeroesAnalytics.Models.ScoreCardModels;
using CricHeroesAnalytics.Repositories;
using CricHeroesAnalytics.Services.Interfaces;

namespace CricHeroesAnalytics.Services
{
    public class MatchAnalyticService : IMatchAnalyticService
    {
        private const long Cult100TeamId = 5455774;
        private readonly ILogger _logger;
        private readonly ICricHeroesApiClient cricHeroesApiClient;
        private readonly IMatchRepository _matchRepository;
        private readonly IPlayerAnalyticsService _playerAnalyticsService;

        public MatchAnalyticService(
            ILogger<IMatchAnalyticService> logger,
            ICricHeroesApiClient cricHeroesApiClient,
            IMatchRepository matchRepository,
            IPlayerAnalyticsService playerAnalyticsService)
        {
            _logger = logger;
            this.cricHeroesApiClient = cricHeroesApiClient;
            this._matchRepository = matchRepository;
            this._playerAnalyticsService = playerAnalyticsService;
        }

        public async Task UpdateLatestMatchData()
        {
            List<MatchData> matchData = await cricHeroesApiClient.GetMatches();
            if (matchData == null)
            {
                _logger.LogInformation("No matches found");
                return;
            }
            matchData = FilterValidMatches(matchData);
            matchData.Sort(Compare);
            foreach (MatchData data in matchData)
            {
                string matchIdString = data.MatchId.ToString();
                bool alreadyUpdated = await this._matchRepository.IsMatchAlreadyUpdated(matchIdString);
                
                // Matches already updated.
                if (alreadyUpdated)
                {
                    _logger.LogInformation($"Match Already Updated {matchIdString}");
                    break;
                }
                ScoreCardResponse scoreCardResponse = await cricHeroesApiClient.GetScoreCard(data);
                if (scoreCardResponse == null)
                {
                    _logger.LogInformation($"ScoreCard Not Found {matchIdString}");
                    break;
                }
                await this._playerAnalyticsService.UpdatePlayerStatsForMatch(matchIdString, scoreCardResponse.PageProps.Scorecard);
                Match match = new Match();
                match.MatchId = matchIdString;
                match.Id = matchIdString;
                if (scoreCardResponse.PageProps.Scorecard[0].TeamId == Cult100TeamId)
                {
                    match.Batting = scoreCardResponse.PageProps.Scorecard[0].Batting;
                    match.Bowling = scoreCardResponse.PageProps.Scorecard[1].Bowling;
                } else
                {
                    match.Batting = scoreCardResponse.PageProps.Scorecard[1].Batting;
                    match.Bowling = scoreCardResponse.PageProps.Scorecard[0].Bowling;
                }
                await this._matchRepository.AddMatch(match);
            }
        }

        private List<MatchData> FilterValidMatches(List<MatchData> matchData)
        {
            List<MatchData> validMatches = new List<MatchData>();
            foreach (var match in matchData)
            {
                if (match.MatchResult.Equals("Resulted") && match.Status.Equals("past") && match.MatchStartTime > DateTime.Now.AddMonths(-1))
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
