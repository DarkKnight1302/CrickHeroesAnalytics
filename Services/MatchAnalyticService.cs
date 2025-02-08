using CricHeroesAnalytics.Constants;
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
        private readonly IJobExecutionRepository _jobExecutionRepository;
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public MatchAnalyticService(
            ILogger<IMatchAnalyticService> logger,
            ICricHeroesApiClient cricHeroesApiClient,
            IMatchRepository matchRepository,
            IPlayerAnalyticsService playerAnalyticsService,
            IJobExecutionRepository jobExecutionRepository)
        {
            _logger = logger;
            this.cricHeroesApiClient = cricHeroesApiClient;
            this._matchRepository = matchRepository;
            this._playerAnalyticsService = playerAnalyticsService;
            this._jobExecutionRepository = jobExecutionRepository;
        }

        public async Task UpdateLatestMatchData()
        {
            await semaphore.WaitAsync();
            try
            {
                string JobName = GlobalConstants.CustomUpdateScoreJob;
                string jobId = $"{JobName}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
                await this._jobExecutionRepository.StartJobExecution(jobId, JobName);
                List<MatchData> matchData = await cricHeroesApiClient.GetMatches();
                if (matchData == null || matchData.Count == 0)
                {
                    await this._jobExecutionRepository.JobFailed(jobId, "Error in Fetching match data, not found");
                    _logger.LogInformation("No matches found");
                    return;
                }
                await this._jobExecutionRepository.JobSucceeded(jobId);
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
                    if (scoreCardResponse == null || scoreCardResponse.PageProps.Scorecard[0].Batting.Count == 0)
                    {
                        _logger.LogInformation($"ScoreCard Not Found {matchIdString}");
                        break;
                    }

                    await this._playerAnalyticsService.UpdatePlayerStatsForMatch(matchIdString, scoreCardResponse.PageProps.Scorecard, data.MatchStartTime);

                    Match match = new Match();
                    match.MatchId = matchIdString;
                    match.Id = matchIdString;
                    match.matchData = data;
                    if (scoreCardResponse.PageProps.Scorecard[0].TeamId == Cult100TeamId)
                    {
                        match.Batting = scoreCardResponse.PageProps.Scorecard[0].Batting;
                        match.Bowling = scoreCardResponse.PageProps.Scorecard[1].Bowling;
                    }
                    else
                    {
                        match.Batting = scoreCardResponse.PageProps.Scorecard[1].Batting;
                        match.Bowling = scoreCardResponse.PageProps.Scorecard[0].Bowling;
                    }
                    await this._matchRepository.AddMatch(match);
                }
            } finally
            {
                semaphore.Release();
            }
        }

        private List<MatchData> FilterValidMatches(List<MatchData> matchData)
        {
            List<MatchData> validMatches = new List<MatchData>();
            foreach (var match in matchData)
            {
                if (match.MatchResult.Equals("Resulted") && match.Status.Equals("past") && match.MatchStartTime > DateTime.Now.AddDays(-15))
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

        /*private async Task UpdateMatchData(List<MatchData> matchData)
        {
            List<Match> storedMatches = await this._matchRepository.GetAllMatches();
            foreach(Match match in storedMatches)
            {
                if (match.matchData != null)
                {
                    ScoreCardResponse res = await this.cricHeroesApiClient.GetScoreCard(match.matchData);
                    Scorecard s1 = res.PageProps.Scorecard[0];
                    Scorecard s2 = res.PageProps.Scorecard[1];
                    if (s1.TeamId == 5455774)
                    {
                        foreach(var batting in s1.Batting)
                        {
                            int playerId = batting.PlayerId;
                            Entities.Player p = await this.playerRepository.GetPlayer(playerId.ToString());
                            if (match.matchData.MatchStartTime > p.LastMatchUpdated)
                            {
                                p.LastMatchUpdated = match.matchData.MatchStartTime;
                                await this.playerRepository.CreateOrUpdatePlayer(p);
                            }
                        }
                        foreach (var batting in s2.Bowling)
                        {
                            int playerId = batting.PlayerId;
                            Entities.Player p = await this.playerRepository.GetPlayer(playerId.ToString());
                            if (match.matchData.MatchStartTime > p.LastMatchUpdated)
                            {
                                p.LastMatchUpdated = match.matchData.MatchStartTime;
                                await this.playerRepository.CreateOrUpdatePlayer(p);
                            }
                        }
                    }
                    if (s2.TeamId == 5455774)
                    {
                        foreach (var batting in s2.Batting)
                        {
                            int playerId = batting.PlayerId;
                            Entities.Player p = await this.playerRepository.GetPlayer(playerId.ToString());
                            if (match.matchData.MatchStartTime > p.LastMatchUpdated)
                            {
                                p.LastMatchUpdated = match.matchData.MatchStartTime;
                                await this.playerRepository.CreateOrUpdatePlayer(p);
                            }
                        }
                        foreach (var batting in s1.Bowling)
                        {
                            int playerId = batting.PlayerId;
                            Entities.Player p = await this.playerRepository.GetPlayer(playerId.ToString());
                            if (match.matchData.MatchStartTime > p.LastMatchUpdated)
                            {
                                p.LastMatchUpdated = match.matchData.MatchStartTime;
                                await this.playerRepository.CreateOrUpdatePlayer(p);
                            }
                        }
                    }
                }    
            }
        }*/
    }

}
