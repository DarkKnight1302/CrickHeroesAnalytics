using CricHeroesAnalytics.Entities;
using CricHeroesAnalytics.Services.Interfaces;
using Microsoft.Azure.Cosmos;

namespace CricHeroesAnalytics.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly ICosmosDbService cosmosDbService;
        private readonly ILogger<MatchRepository> logger;

        public MatchRepository(ICosmosDbService cosmosDbService, ILogger<MatchRepository> logger)
        {
            this.cosmosDbService = cosmosDbService;
            this.logger = logger;
        }
        public async Task AddMatch(Match match)
        {
            var container = FetchContainer();
            await container.UpsertItemAsync<Match>(match, new PartitionKey(match.Id)).ConfigureAwait(false);
        }

        public async Task<bool> IsMatchAlreadyUpdated(string matchId)
        {
            var container = FetchContainer();
            try
            {
                ItemResponse<Match> response = await container.ReadItemAsync<Match>(matchId, new PartitionKey(matchId));
                if (response != null && response.Resource != null)
                {
                    return true;
                }
            }
            catch (CosmosException)
            {
                this.logger.LogInformation($"Match Doesn't exist {matchId}");
                return false;
            }
            return false;
        }

        private Container FetchContainer()
        {
            return this.cosmosDbService.GetContainer("Matches");
        }
    }
}
