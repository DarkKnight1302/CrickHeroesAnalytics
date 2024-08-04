namespace CricHeroesAnalytics.Repositories
{
    public interface IJobExecutionRepository
    {
        public Task StartJobExecution(string jobId, string JobName);

        public Task JobSucceeded(string jobId);

        public Task JobFailed(string jobId, string reason);
    }
}
