namespace CricHeroesAnalytics.Models
{
    public class TeamPerformance
    {
        public double WinPercentage { get; set; }
        public int TotalMatches { get; set; }
        public int MatchesWon { get; set; }
        public int MatchesLost { get; set; }
        public string TeamName { get; set; } = "Cult 11";
        public string Period { get; set; } = "Last 6 Months";
    }
}