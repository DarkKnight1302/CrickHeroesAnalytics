using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models
{
    public class TeamData
    {
        [JsonProperty("team_id")]
        public int TeamId { get; set; }

        [JsonProperty("team_name")]
        public string TeamName { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("city_name")]
        public string CityName { get; set; }

        [JsonProperty("city_id")]
        public int CityId { get; set; }

        [JsonProperty("captain_id")]
        public object CaptainId { get; set; }

        [JsonProperty("created_by")]
        public int CreatedBy { get; set; }

        [JsonProperty("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("is_verified")]
        public int IsVerified { get; set; }

        [JsonProperty("is_active")]
        public int IsActive { get; set; }

        [JsonProperty("club_id")]
        public string ClubId { get; set; }

        [JsonProperty("club_name")]
        public string ClubName { get; set; }

        [JsonProperty("club_admin_id")]
        public string ClubAdminId { get; set; }

        [JsonProperty("is_web_view")]
        public int IsWebView { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("arrange_match_status")]
        public string ArrangeMatchStatus { get; set; }

        [JsonProperty("mapping_id")]
        public int MappingId { get; set; }

        [JsonProperty("statement")]
        public string Statement { get; set; }

        [JsonProperty("player_name")]
        public string PlayerName { get; set; }

        [JsonProperty("profile_photo")]
        public string ProfilePhoto { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("arrange_match_status_note")]
        public string ArrangeMatchStatusNote { get; set; }

        [JsonProperty("is_follow")]
        public int IsFollow { get; set; }

        [JsonProperty("is_verified_team_popup_text")]
        public string IsVerifiedTeamPopupText { get; set; }

        [JsonProperty("member_nudge_info")]
        public MemberNudgeInfo MemberNudgeInfo { get; set; }

        [JsonProperty("share_message_profile")]
        public string ShareMessageProfile { get; set; }

        [JsonProperty("share_message_leaderboard")]
        public string ShareMessageLeaderboard { get; set; }

        [JsonProperty("share_message_qr")]
        public string ShareMessageQr { get; set; }

        [JsonProperty("share_message_members")]
        public string ShareMessageMembers { get; set; }

        [JsonProperty("share_message_stats")]
        public string ShareMessageStats { get; set; }

        [JsonProperty("share_message_matches")]
        public string ShareMessageMatches { get; set; }

        [JsonProperty("is_club_team_admin")]
        public int IsClubTeamAdmin { get; set; }

        [JsonProperty("is_award_winner")]
        public int IsAwardWinner { get; set; }

        [JsonProperty("awards_link")]
        public string AwardsLink { get; set; }
    }
}
