using CricHeroesAnalytics.Entities;

namespace CricHeroesAnalytics.Repositories
{
    public interface IPlayerRepository
    {
        public Task CreateOrUpdatePlayer(Player player);

        public Task<Player> GetPlayer(string playerId);
    }
}
