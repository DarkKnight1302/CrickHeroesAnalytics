using CricHeroesAnalytics.Entities;
using CricHeroesAnalytics.Services.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace CricHeroesAnalytics.Repositories
{
    public class GroundSlotRepository : IGroundSlotRepository
    {
        private readonly ICosmosDbService cosmosDbService;
        private readonly ILogger<IGroundSlotRepository> logger;

        public GroundSlotRepository(ICosmosDbService cosmosDbService, ILogger<IGroundSlotRepository> logger)
        {
            this.logger = logger;
            this.cosmosDbService = cosmosDbService;
        }
        public List<GroundSlot> GetAllAvailableGroundSlots()
        {
            List<GroundSlot> availableSlots = new List<GroundSlot>();
            var container = FetchContainer();
            var q = container.GetItemLinqQueryable<GroundSlot>(true);
            var enumerator = q.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.IsAvailable)
                {
                    availableSlots.Add(enumerator.Current);
                }
            }
            return (List<GroundSlot>)availableSlots.OrderBy(x => x.DistanceInKm);
        }

        public async Task<List<GroundSlot>> GetAllGroundSlotsAsync()
        {
            List<GroundSlot> availableSlots = new List<GroundSlot>();
            var container = FetchContainer();
            var q = container.GetItemLinqQueryable<GroundSlot>();
            var feedIterator = q.ToFeedIterator();
            while(feedIterator.HasMoreResults)
            {
                FeedResponse<GroundSlot> feedResponse = await feedIterator.ReadNextAsync();
                availableSlots.AddRange(feedResponse);
            }
            return availableSlots;
        }

        public async Task<GroundSlot> GetGroundSlotAsync(string groundName)
        {
            Container container = FetchContainer();
            try
            {
                ItemResponse<GroundSlot> itemResponse = await container.ReadItemAsync<GroundSlot>(groundName, new PartitionKey(groundName));
                if (itemResponse != null)
                {
                    return itemResponse.Resource;
                }
            } catch(CosmosException)
            {
                this.logger.LogInformation("Ground Details Not found");
            }
            return null;
        }

        public async Task UpdateGroundSlot(GroundSlot groundSlot)
        {
            var container = FetchContainer();
            await container.UpsertItemAsync<GroundSlot>(groundSlot, new PartitionKey(groundSlot.Id));
        }

        private Container FetchContainer()
        {
            return this.cosmosDbService.GetContainer("GroundSlots");
        }
    }
}
