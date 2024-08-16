using CricHeroesAnalytics.Entities;

namespace CricHeroesAnalytics.Repositories
{
    public interface IGroundSlotRepository
    {
        public List<GroundSlot> GetAllAvailableGroundSlots();

        public Task<List<GroundSlot>> GetAllGroundSlotsAsync();

        public Task UpdateGroundSlot(GroundSlot groundSlot);

        public Task<GroundSlot> GetGroundSlotAsync(string groundName);
    }
}
