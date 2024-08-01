using CricHeroesAnalytics.Entities;
using CricHeroesAnalytics.Services.Interfaces;
using Microsoft.Azure.Cosmos;

namespace CricHeroesAnalytics.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly ILogger _logger;
        private readonly ICosmosDbService cosmosDbService;

        public PlayerRepository(ICosmosDbService cosmosDbService, ILogger<PlayerRepository> logger)
        {
            this.cosmosDbService = cosmosDbService;
            this._logger = logger;
        }
        public async Task CreateOrUpdatePlayer(Player player)
        {
            var container = FetchContainer();
            await container.UpsertItemAsync<Player>(player, new PartitionKey(player.Id));
        }

        public async Task<Player> GetPlayer(string playerId)
        {
            var container = FetchContainer();
            try
            {
                ItemResponse<Player> playerResponse = await container.ReadItemAsync<Player>(playerId, new PartitionKey(playerId));
                return playerResponse.Resource;
            }
            catch (CosmosException)
            {
                this._logger.LogInformation($"Player Not found {playerId}");
            }
            return null;
        }

        private Container FetchContainer()
        {
            return this.cosmosDbService.GetContainer("Players");
        }
    }
}
