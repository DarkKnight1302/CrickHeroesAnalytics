using CricHeroesAnalytics.Entities;

namespace CricHeroesAnalytics.Services.Interfaces
{
    public interface IPlayerStatService
    {
        public double GetAverageFromLastNInnings(Player player, int n);

        public int GetStrikeRateFromLastNInnings(Player player, int n);

        public double GetEconomyFromLastNInnings(Player player, int n);
    }
}
