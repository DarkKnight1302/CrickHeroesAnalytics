using CricHeroesAnalytics.Entities;
using CricHeroesAnalytics.Services.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System;

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

        public List<Player> GetAllPlayers()
        {
            List<Player> players = new List<Player>();
            var container = FetchContainer();
            var q = container.GetItemLinqQueryable<Player>(true);
            var enumerator = q.GetEnumerator();
            while (enumerator.MoveNext())
            {
                players.Add(enumerator.Current);
            }
            return players;
        }

        public async Task<List<Player>> GetAllPlayersAsync()
        {
            List<Player> players = new List<Player>();
            var container = FetchContainer();
            var q = container.GetItemLinqQueryable<Player>(true);
            var iterator = q.ToFeedIterator();
            while (iterator.HasMoreResults)
            {
                FeedResponse<Player> feedResponse = await iterator.ReadNextAsync();
                players.AddRange(feedResponse);
            }
            return players;
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
