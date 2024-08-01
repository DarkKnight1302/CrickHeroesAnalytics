using CricHeroesAnalytics.Services.Interfaces;
using Quartz;

namespace CricHeroesAnalytics.CronJob
{
    public class ScoreUpdateJob : IJob
    {
        private readonly IMatchAnalyticService matchAnalyticService;
        private readonly ILogger logger;

        public ScoreUpdateJob(IMatchAnalyticService matchAnalyticService, ILogger<ScoreUpdateJob> logger) 
        {
            this.matchAnalyticService = matchAnalyticService;
            this.logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            this.logger.LogInformation("Starting cron job");
            await this.matchAnalyticService.UpdateLatestMatchData();
            this.logger.LogInformation("Cron job completed");
        }
    }
}
