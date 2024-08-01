using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models.ScoreCardModels
{
    public class UpcomingResponse
    {
        [JsonProperty("match_id")]
        public int MatchId { get; set; }

        [JsonProperty("match_type")]
        public string MatchType { get; set; }

        [JsonProperty("team1_id")]
        public int Team1Id { get; set; }

        [JsonProperty("team1_name")]
        public string Team1Name { get; set; }

        [JsonProperty("team2_id")]
        public int Team2Id { get; set; }

        [JsonProperty("team2_name")]
        public string Team2Name { get; set; }

        [JsonProperty("series_id")]
        public int SeriesId { get; set; }

        [JsonProperty("series_name")]
        public string SeriesName { get; set; }

        [JsonProperty("season")]
        public string Season { get; set; }

        [JsonProperty("match_number")]
        public string MatchNumber { get; set; }

        [JsonProperty("match_date")]
        public DateTime MatchDate { get; set; }

        [JsonProperty("match_time")]
        public string MatchTime { get; set; }

        [JsonProperty("venue_id")]
        public int VenueId { get; set; }

        [JsonProperty("venue_name")]
        public string VenueName { get; set; }

        [JsonProperty("match_status")]
        public string MatchStatus { get; set; }
    }
}
