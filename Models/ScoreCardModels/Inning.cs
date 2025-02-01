using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models.ScoreCardModels
{
    public class Inning
    {
        [JsonProperty("team_id")]
        public int TeamId { get; set; }

        [JsonProperty("inning")]
        public int InningNumber { get; set; }

        [JsonProperty("inning_start_time")]
        public DateTime? InningStartTime { get; set; }

        [JsonProperty("inning_end_time")]
        public DateTime? InningEndTime { get; set; }

        [JsonProperty("is_declare")]
        public int IsDeclare { get; set; }

        [JsonProperty("is_forfeited")]
        public int IsForfeited { get; set; }

        [JsonProperty("is_followon")]
        public int IsFollowon { get; set; }

        [JsonProperty("is_allout")]
        public int IsAllout { get; set; }

        [JsonProperty("total_run")]
        public int TotalRun { get; set; }

        [JsonProperty("total_wicket")]
        public int TotalWicket { get; set; }

        [JsonProperty("total_extra")]
        public int TotalExtra { get; set; }

        [JsonProperty("overs_played")]
        public string OversPlayed { get; set; }

        [JsonProperty("balls_played")]
        public int BallsPlayed { get; set; }

        [JsonProperty("revised_target")]
        public int RevisedTarget { get; set; }

        [JsonProperty("revised_overs")]
        public int RevisedOvers { get; set; }

        [JsonProperty("penalty_run")]
        public int PenaltyRun { get; set; }

        [JsonProperty("lead_by")]
        public int LeadBy { get; set; }

        [JsonProperty("trail_by")]
        public int TrailBy { get; set; }

        [JsonProperty("negative_run")]
        public int NegativeRun { get; set; }

        [JsonProperty("summary")]
        public Summary Summary { get; set; }
    }
}
