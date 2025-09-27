namespace CricHeroesAnalytics.Services.Interfaces
{
    public interface ITeamAnalyticsService
    {
        Task<double> GetTeamWinPercentageAsync(string teamId, int monthsBack = 6);
        Task<int> GetTotalMatchesPlayedAsync(string teamId, int monthsBack = 6);
        Task<int> GetMatchesWonAsync(string teamId, int monthsBack = 6);
        Task<int> GetMatchesLostAsync(string teamId, int monthsBack = 6);
    }
}