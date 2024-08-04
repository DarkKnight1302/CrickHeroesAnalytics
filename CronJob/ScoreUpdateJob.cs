using CricHeroesAnalytics.Repositories;
using CricHeroesAnalytics.Services.Interfaces;
using Quartz;
using static Quartz.Logging.OperationName;
using System;

namespace CricHeroesAnalytics.CronJob
{
    public class ScoreUpdateJob : IJob
    {
        private readonly IMatchAnalyticService matchAnalyticService;
        private readonly ILogger logger;
        private readonly IJobExecutionRepository jobExecutionRepository;

        public ScoreUpdateJob(IMatchAnalyticService matchAnalyticService, ILogger<ScoreUpdateJob> logger, IJobExecutionRepository jobExecutionRepository) 
        {
            this.matchAnalyticService = matchAnalyticService;
            this.logger = logger;
            this.jobExecutionRepository = jobExecutionRepository;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            string JobName = context.JobDetail.Key.Name;
            string jobId = $"{JobName}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
            try
            {
                await this.jobExecutionRepository.StartJobExecution(jobId, JobName);
                this.logger.LogInformation("Starting cron job");
                await this.matchAnalyticService.UpdateLatestMatchData();
                this.logger.LogInformation("Cron job completed");
                await this.jobExecutionRepository.JobSucceeded(jobId);
            } catch (Exception ex)
            {
                string reason = $"Job execution failed {ex.Message} \n {ex.StackTrace}";
                this.logger.LogError(reason);
                await this.jobExecutionRepository.JobFailed(jobId, reason);
            }
        }
    }
}
