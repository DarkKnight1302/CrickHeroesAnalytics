namespace CricHeroesAnalytics.Entities
{
    public class PlayerWicketsPerMatch
    {
        public string MatchId { get; set; }

        public int Wickets {  get; set; }

        public int RunsGiven { get; set; }

        public double Overs {  get; set; }
    }
}
