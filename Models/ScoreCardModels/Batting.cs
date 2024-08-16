using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models.ScoreCardModels
{
    public class Batting
    {
        [JsonProperty("player_id")]
        public int PlayerId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_impact_player_in")]
        public int IsImpactPlayerIn { get; set; }

        [JsonProperty("is_impact_player_out")]
        public int IsImpactPlayerOut { get; set; }

        [JsonProperty("runs")]
        public int Runs { get; set; }

        [JsonProperty("balls")]
        public int Balls { get; set; }

        [JsonProperty("minutes")]
        public int Minutes { get; set; }

        [JsonProperty("4s")]
        public int Fours { get; set; }

        [JsonProperty("6s")]
        public int Sixes { get; set; }

        [JsonProperty("SR")]
        public string StrikeRate { get; set; }

        [JsonProperty("batting_hand")]
        public string BattingHand { get; set; }

        [JsonProperty("highlight_videos")]
        public List<string> HighlightVideos { get; set; }

        [JsonProperty("wicket_videos")]
        public List<string> WicketVideos { get; set; }

        [JsonProperty("short_name")]
        public string ShortName { get; set; }

        [JsonProperty("how_to_out")]
        public string HowToOut { get; set; }

        [JsonProperty("how_to_out_short_name")]
        public string HowToOutShortName { get; set; }
    }

}
