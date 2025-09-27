using CricHeroesAnalytics.Entities;
using CricHeroesAnalytics.Repositories;
using CricHeroesAnalytics.Services.Interfaces;

namespace CricHeroesAnalytics.Services
{
    public class TeamAnalyticsService : ITeamAnalyticsService
    {
        private const long Cult11TeamId = 5455774;
        private readonly IMatchRepository _matchRepository;
        private readonly ILogger<TeamAnalyticsService> _logger;

        public TeamAnalyticsService(IMatchRepository matchRepository, ILogger<TeamAnalyticsService> logger)
        {
            _matchRepository = matchRepository;
            _logger = logger;
        }

        public async Task<double> GetTeamWinPercentageAsync(string teamId, int monthsBack = 6)
        {
            var totalMatches = await GetTotalMatchesPlayedAsync(teamId, monthsBack);
            if (totalMatches == 0)
            {
                return 0.0;
            }

            var matchesWon = await GetMatchesWonAsync(teamId, monthsBack);
            return Math.Round((double)matchesWon / totalMatches * 100, 1);
        }

        public async Task<int> GetTotalMatchesPlayedAsync(string teamId, int monthsBack = 6)
        {
            var cutoffDate = DateTime.Now.AddMonths(-monthsBack);
            var allMatches = await _matchRepository.GetAllMatches();
            
            return allMatches.Count(m => 
                m.matchData != null && 
                m.matchData.MatchStartTime >= cutoffDate &&
                m.matchData.Status.Equals("past", StringComparison.OrdinalIgnoreCase) &&
                m.matchData.MatchResult.Equals("Resulted", StringComparison.OrdinalIgnoreCase) &&
                (m.matchData.TeamAId == Cult11TeamId || m.matchData.TeamBId == Cult11TeamId));
        }

        public async Task<int> GetMatchesWonAsync(string teamId, int monthsBack = 6)
        {
            var cutoffDate = DateTime.Now.AddMonths(-monthsBack);
            var allMatches = await _matchRepository.GetAllMatches();
            
            return allMatches.Count(m => 
                m.matchData != null && 
                m.matchData.MatchStartTime >= cutoffDate &&
                m.matchData.Status.Equals("past", StringComparison.OrdinalIgnoreCase) &&
                m.matchData.MatchResult.Equals("Resulted", StringComparison.OrdinalIgnoreCase) &&
                (m.matchData.TeamAId == Cult11TeamId || m.matchData.TeamBId == Cult11TeamId) &&
                !string.IsNullOrEmpty(m.matchData.WinningTeamId) &&
                m.matchData.WinningTeamId == Cult11TeamId.ToString());
        }

        public async Task<int> GetMatchesLostAsync(string teamId, int monthsBack = 6)
        {
            var totalMatches = await GetTotalMatchesPlayedAsync(teamId, monthsBack);
            var matchesWon = await GetMatchesWonAsync(teamId, monthsBack);
            return totalMatches - matchesWon;
        }
    }
}