namespace CricHeroesAnalytics.Models.ScoreCardModels
{
    public class BattingPerformance
    {
        public int team_id { get; set; }
        public string team_name { get; set; }
        public int player_id { get; set; }
        public string player_name { get; set; }
        public int runs { get; set; }
        public int balls { get; set; }
        public int _4s { get; set; }
        public int _6s { get; set; }
        public string strike_rate { get; set; }
        public int bonus_run { get; set; }
        public int is_out { get; set; }
        public int inning { get; set; }
        public int is_impact_player_in { get; set; }
        public int is_impact_player_out { get; set; }
    }
}
