namespace CricHeroesAnalytics.Models.ScoreCardModels
{
    public class MatchNote
    {
        public int team_id { get; set; }
        public int inning { get; set; }
        public int day { get; set; }
        public int is_scorer_note { get; set; }
        public string note { get; set; }
    }
}
