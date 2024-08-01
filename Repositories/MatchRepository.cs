using CricHeroesAnalytics.Entities;
using CricHeroesAnalytics.Services.Interfaces;
using Microsoft.Azure.Cosmos;

namespace CricHeroesAnalytics.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly ICosmosDbService cosmosDbService;

        public MatchRepository(ICosmosDbService cosmosDbService)
        {
            this.cosmosDbService = cosmosDbService;
        }
        public async Task AddMatch(Match match)
        {
            var container = FetchContainer();
            await container.CreateItemAsync<Match>(match, new PartitionKey(match.Id)).ConfigureAwait(false);
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
