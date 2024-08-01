using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models
{
    public class MatchData
    {
        [JsonProperty("match_id")]
        public int MatchId { get; set; }

        [JsonProperty("match_type")]
        public string MatchType { get; set; }

        [JsonProperty("is_super_over")]
        public int IsSuperOver { get; set; }

        [JsonProperty("match_event_type")]
        public string MatchEventType { get; set; }

        [JsonProperty("match_event")]
        public string MatchEvent { get; set; }

        [JsonProperty("match_type_id")]
        public int MatchTypeId { get; set; }

        [JsonProperty("match_inning")]
        public int MatchInning { get; set; }

        [JsonProperty("ball_type")]
        public string BallType { get; set; }

        [JsonProperty("current_inning")]
        public int CurrentInning { get; set; }

        [JsonProperty("match_start_time")]
        public DateTime MatchStartTime { get; set; }

        [JsonProperty("match_end_time")]
        public string MatchEndTime { get; set; }

        [JsonProperty("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("created_by")]
        public int CreatedBy { get; set; }

        [JsonProperty("city_id")]
        public int CityId { get; set; }

        [JsonProperty("city_name")]
        public string CityName { get; set; }

        [JsonProperty("ground_id")]
        public int GroundId { get; set; }

        [JsonProperty("ground_name")]
        public string GroundName { get; set; }

        [JsonProperty("overs")]
        public int Overs { get; set; }

        [JsonProperty("balls")]
        public object Balls { get; set; }

        [JsonProperty("over_reduce")]
        public string OverReduce { get; set; }

        [JsonProperty("is_dl")]
        public int IsDl { get; set; }

        [JsonProperty("is_vjd")]
        public int IsVjd { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("winning_team_id")]
        public string WinningTeamId { get; set; }

        [JsonProperty("winning_team")]
        public string WinningTeam { get; set; }

        [JsonProperty("match_result")]
        public string MatchResult { get; set; }

        [JsonProperty("win_by")]
        public string WinBy { get; set; }

        [JsonProperty("team_a_id")]
        public int TeamAId { get; set; }

        [JsonProperty("team_a")]
        public string TeamA { get; set; }

        [JsonProperty("team_a_logo")]
        public string TeamALogo { get; set; }

        [JsonProperty("is_a_home_team")]
        public int IsAHomeTeam { get; set; }

        [JsonProperty("team_b_id")]
        public int TeamBId { get; set; }

        [JsonProperty("team_b")]
        public string TeamB { get; set; }

        [JsonProperty("team_b_logo")]
        public string TeamBLogo { get; set; }

        [JsonProperty("is_b_home_team")]
        public int IsBHomeTeam { get; set; }

        [JsonProperty("invite_other_teams")]
        public int InviteOtherTeams { get; set; }

        [JsonProperty("score_calculate")]
        public int ScoreCalculate { get; set; }

        [JsonProperty("highlight_player_ids")]
        public string HighlightPlayerIds { get; set; }

        [JsonProperty("highlight_players")]
        public string HighlightPlayers { get; set; }

        [JsonProperty("officials")]
        public List<object> Officials { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("team_a_short_name")]
        public string TeamAShortName { get; set; }

        [JsonProperty("team_b_short_name")]
        public string TeamBShortName { get; set; }

        [JsonProperty("match_full_title")]
        public string MatchFullTitle { get; set; }

        [JsonProperty("match_short_title")]
        public string MatchShortTitle { get; set; }

        [JsonProperty("match_category")]
        public string MatchCategory { get; set; }

        [JsonProperty("event_name")]
        public string EventName { get; set; }

        [JsonProperty("event_short_name")]
        public string EventShortName { get; set; }

        [JsonProperty("show_in_homepage")]
        public int ShowInHomepage { get; set; }

        [JsonProperty("p1")]
        public int P1 { get; set; }

        [JsonProperty("p2")]
        public int P2 { get; set; }

        [JsonProperty("cricket_bat")]
        public int CricketBat { get; set; }

        [JsonProperty("team_a_no_of_balls")]
        public int TeamANoOfBalls { get; set; }

        [JsonProperty("team_b_no_of_balls")]
        public int TeamBNoOfBalls { get; set; }
    }

}
