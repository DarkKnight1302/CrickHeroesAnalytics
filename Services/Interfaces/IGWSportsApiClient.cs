using CricHeroesAnalytics.Models.GWModels;

namespace CricHeroesAnalytics.Services.Interfaces
{
    public interface IGWSportsApiClient
    {
        public Task<GroundSlots> GetGroundSlotsAsync(string ground, DateTimeOffset dateTime);
    }
}
