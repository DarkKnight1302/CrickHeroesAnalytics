using CricHeroesAnalytics.Entities;
using CricHeroesAnalytics.Services.Interfaces;

namespace CricHeroesAnalytics.Services
{
    public class PlayerRatingService : IPlayerRatingService
    {
        public double GetBattingRating(Player player)
        {
            return player.BattingAverage * (double)player.StrikeRate * (double)player.StrikeRate;
        }

        public double GetBowlingRating(Player player)
        {
            if (player.TotalWickets == 0)
            {
                return 0;
            }
            double bowlingAverage = ((double)player.RunsGiven / (double)player.TotalWickets);
            player.BowlingAverage = Math.Round(bowlingAverage, 1);
            return 1d / (bowlingAverage);
        }
    }
}
