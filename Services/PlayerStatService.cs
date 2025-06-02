using CricHeroesAnalytics.Entities;
using CricHeroesAnalytics.Services.Interfaces;
using System.Threading;

namespace CricHeroesAnalytics.Services
{
    public class PlayerStatService : IPlayerStatService
    {
        private readonly ILogger<IPlayerStatService> logger;

        public PlayerStatService(ILogger<IPlayerStatService> logger)
        {
            this.logger = logger;
        }

        public double GetAverageFromLastNInnings(Player player, int n)
        {
            if (player?.PlayerRunMatchMap == null || player.PlayerRunMatchMap.Count == 0 || n <= 0)
                return 0;

            // Sort keys lexicographically and take top n
            var topNMatches = player.PlayerRunMatchMap
                .OrderByDescending(kvp => kvp.Key)
                .Take(n)
                .Select(kvp => kvp.Value)
                .ToList();

            if (topNMatches.Count == 0)
                return 0;

            int totalRuns = topNMatches.Sum(x => x.Runs);
            int timesOut = topNMatches.Count(x => !x.WasNotOut);
            //logger.LogInformation($"{player.Name} - {totalRuns} : {timesOut}");
            // Avoid division by zero
            return timesOut > 0 ? (double)totalRuns / timesOut : totalRuns;

        }

        public double GetEconomyFromLastNInnings(Player player, int n)
        {
            if (player?.PlayerWicketsMatchMap == null || player.PlayerWicketsMatchMap.Count == 0 || n <= 0)
                return 0;

            // Sort keys lexicographically (descending) and take top n
            var topNMatches = player.PlayerWicketsMatchMap
                .OrderByDescending(kvp => kvp.Key)
                .Take(n)
                .Select(kvp => kvp.Value)
                .ToList();

            int totalRunsGiven = topNMatches.Sum(x => x.RunsGiven);
            double totalOvers = topNMatches.Sum(x => x.Overs);

            // Economy = total runs given / total overs bowled
            if (totalOvers == 0)
                return 0;

            return totalRunsGiven / totalOvers;
        }

        public int GetStrikeRateFromLastNInnings(Player player, int n)
        {
            if (player?.PlayerRunMatchMap == null || player.PlayerRunMatchMap.Count == 0 || n <= 0)
                return 0;

            // Sort keys lexicographically (descending) and take top n
            var topNMatches = player.PlayerRunMatchMap
                .OrderByDescending(kvp => kvp.Key)
                .Take(n)
                .Select(kvp => kvp.Value)
                .ToList();

            int totalRuns = topNMatches.Sum(x => x.Runs);
            int totalBalls = topNMatches.Sum(x => x.BallsPlayed);

            // Strike rate = (total runs * 100) / total balls played
            if (totalBalls == 0)
                return 0;
            //logger.LogInformation($"{player.Name} - {totalRuns} : {totalBalls}");
            return (int)Math.Round((double)totalRuns * 100 / totalBalls);
        }
    }
}
