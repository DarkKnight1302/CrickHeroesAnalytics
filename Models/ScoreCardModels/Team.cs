namespace CricHeroesAnalytics.Models.ScoreCardModels
{
    public class Team
    {
        public int id { get; set; }
        public string name { get; set; }
        public string logo { get; set; }
        public int is_home_team { get; set; }
        public string summary { get; set; }
        public List<Inning> innings { get; set; }
    }
}
