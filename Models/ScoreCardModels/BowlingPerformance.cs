namespace CricHeroesAnalytics.Models.ScoreCardModels
{
    public class BowlingPerformance
    {
        public int team_id { get; set; }
        public string team_name { get; set; }
        public int player_id { get; set; }
        public string player_name { get; set; }
        public string overs { get; set; }
        public int balls { get; set; }
        public int maidens { get; set; }
        public int _0s { get; set; }
        public int runs { get; set; }
        public int wickets { get; set; }
        public string economy_rate { get; set; }
        public int inning { get; set; }
        public int bonus_run { get; set; }
        public int is_impact_player_in { get; set; }
        public int is_impact_player_out { get; set; }
    }
}
