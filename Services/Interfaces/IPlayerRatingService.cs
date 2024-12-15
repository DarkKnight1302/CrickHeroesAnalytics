using CricHeroesAnalytics.Entities;

namespace CricHeroesAnalytics.Services.Interfaces
{
    public interface IPlayerRatingService
    {
        public double GetBattingRating(Player player);

        public double GetBowlingRating(Player player);
    }
}
