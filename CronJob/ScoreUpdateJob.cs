using CricHeroesAnalytics.Repositories;
using CricHeroesAnalytics.Services.Interfaces;
using Quartz;
using CricHeroesAnalytics.Entities;
using CricHeroesAnalytics.Models.ScoreCardModels;
using System.Runtime.Intrinsics.Arm;
using CricHeroesAnalytics.Constants;

namespace CricHeroesAnalytics.CronJob
{
    public class ScoreUpdateJob : IJob
    {
        private readonly IMatchAnalyticService matchAnalyticService;
        private readonly ILogger logger;
        private readonly IJobExecutionRepository jobExecutionRepository;
        private readonly IGwGroundAnalyticsService gwGroundAnalyticsService;
        private readonly IPlayerRepository playerRepository;
        private readonly IMatchRepository matchRepository;

        public ScoreUpdateJob(IMatchAnalyticService matchAnalyticService, ILogger<ScoreUpdateJob> logger, IJobExecutionRepository jobExecutionRepository, IGwGroundAnalyticsService gwGroundAnalyticsService, IMatchRepository matchRepository, IPlayerRepository playerRepository) 
        {
            this.matchAnalyticService = matchAnalyticService;
            this.logger = logger;
            this.jobExecutionRepository = jobExecutionRepository;
            this.gwGroundAnalyticsService = gwGroundAnalyticsService;
            this.playerRepository = playerRepository;
            this.matchRepository = matchRepository;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            string JobName = context.JobDetail.Key.Name;
            string jobId = $"{JobName}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
            try
            {
                await this.jobExecutionRepository.StartJobExecution(jobId, JobName);
                this.logger.LogInformation("Starting cron job");
                await this.gwGroundAnalyticsService.UpdateGroundSlots();
                this.logger.LogInformation("Cron job completed");
                await this.jobExecutionRepository.JobSucceeded(jobId);
            } catch (Exception ex)
            {
                string reason = $"Job execution failed {ex.Message} \n {ex.StackTrace}";
                this.logger.LogError(reason);
                await this.jobExecutionRepository.JobFailed(jobId, reason);
            }
        }

        public async Task Correction()
        {
            var players = await this.playerRepository.GetAllPlayersAsync();
            var matches = await this.matchRepository.GetAllMatches();
            Dictionary<string, Dictionary<string, string>> d = new Dictionary<string, Dictionary<string, string>>();
            foreach (Match m in matches)
            {
                Dictionary<string, string> battingOut = new Dictionary<string, string>();
                List<Batting> bt = m.Batting;
                foreach (Batting b in bt)
                {
                    battingOut.TryAdd(b.PlayerId.ToString(), b.HowToOut);
                }
                d.TryAdd(m.MatchId, battingOut);
            }
            foreach(var player in players)
            {
                var map = player.PlayerRunMatchMap;
                foreach(var kv in map)
                {
                    string matchId = kv.Key;
                    PlayerRunsPerMatch playerRunsPerMatch = kv.Value;
                    playerRunsPerMatch.WasNotOut = GlobalConstants.NotOutList.Contains(d[matchId][player.Id]);
                }
                await this.playerRepository.CreateOrUpdatePlayer(player);
            }
        }
    }
}
