﻿
using CricHeroesAnalytics.Entities;
using CricHeroesAnalytics.Extensions;
using CricHeroesAnalytics.Services.Interfaces;
using CricHeroesAnalytics.Utilities;
using Microsoft.Azure.Cosmos;

namespace CricHeroesAnalytics.Repositories
{
    public class JobExecutionRespository : IJobExecutionRepository
    {
        private readonly ICosmosDbService cosmosDbService;
        private readonly ILogger<JobExecutionRespository> logger;

        public JobExecutionRespository(ICosmosDbService cosmosDbService, ILogger<JobExecutionRespository> logger)
        {
            this.cosmosDbService = cosmosDbService;
            this.logger = logger;
        }

        public async Task<DateTimeOffset> GetLastSuccessJobTime(string jobName)
        {
            var container = this.cosmosDbService.GetContainer("JobExecution");

            var query = new QueryDefinition(
                "SELECT TOP 1 c.Created FROM c WHERE c.JobName = @jobName AND c.Status = @status ORDER BY c.Created DESC")
                .WithParameter("@jobName", jobName)
                .WithParameter("@status", Enums.JobStatus.Succeeded.ToString());

            var iterator = container.GetItemQueryIterator<JobExecution>(query);
            var jobExecutions = await iterator.ReadNextAsync();

            var lastSuccessJob = jobExecutions.FirstOrDefault();

            if (lastSuccessJob != null)
            {
                return lastSuccessJob.Created.ToIndiaTime();
            }

            return DateTimeOffset.MinValue;
        }

        public async Task JobFailed(string jobId, string reason)
        {
            var container = this.cosmosDbService.GetContainer("JobExecution");
            JobExecution jobExecution = await GetJobExecutionAsync(jobId);
            jobExecution.Status = Enums.JobStatus.Failed;
            jobExecution.Ended = DateTimeOffset.UtcNow;
            jobExecution.FailureReason = reason;
            await container.UpsertItemAsync(jobExecution, new PartitionKey(jobId));
            this.logger.LogInformation($"Failed job execution {JsonUtil.SerializeObject(jobExecution)}");
        }

        public async Task JobSucceeded(string jobId)
        {
            var container = this.cosmosDbService.GetContainer("JobExecution");
            JobExecution jobExecution = await GetJobExecutionAsync(jobId);
            jobExecution.Status = Enums.JobStatus.Succeeded;
            jobExecution.Ended = DateTimeOffset.UtcNow;
            await container.UpsertItemAsync(jobExecution, new PartitionKey(jobId));
            this.logger.LogInformation($"Succeeded job execution {JsonUtil.SerializeObject(jobExecution)}");
        }

        public async Task StartJobExecution(string jobId, string JobName)
        {
            JobExecution jobExecution = new JobExecution()
            {
                JobId = jobId,
                Id = jobId,
                JobName = JobName,
                Created = DateTimeOffset.UtcNow,
                Status = Enums.JobStatus.Started
            };
            var container = this.cosmosDbService.GetContainer("JobExecution");
            await container.CreateItemAsync<JobExecution>(jobExecution, new PartitionKey(jobId));
            this.logger.LogInformation($"Started job execution {JsonUtil.SerializeObject(jobExecution)}");
        }

        private async Task<JobExecution> GetJobExecutionAsync(string jobId)
        {
            var container = this.cosmosDbService.GetContainer("JobExecution");
            ItemResponse<JobExecution> response = await container.ReadItemAsync<JobExecution>(jobId, new Microsoft.Azure.Cosmos.PartitionKey(jobId));
            return response.Resource;
        }
    }
}
