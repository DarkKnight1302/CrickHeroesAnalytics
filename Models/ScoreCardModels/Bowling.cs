using Newtonsoft.Json;
using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models.ScoreCardModels
{
    public class Bowling
    {
        [JsonProperty("player_id")]
        public int PlayerId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("overs")]
        public double Overs { get; set; }

        [JsonProperty("balls")]
        public int Balls { get; set; }

        [JsonProperty("maidens")]
        public int Maidens { get; set; }

        [JsonProperty("runs")]
        public int Runs { get; set; }

        [JsonProperty("wickets")]
        public int Wickets { get; set; }

        [JsonProperty("0s")]
        public int Zeros { get; set; }

        [JsonProperty("4s")]
        public int Fours { get; set; }

        [JsonProperty("6s")]
        public int Sixes { get; set; }

        [JsonProperty("wide")]
        public int Wide { get; set; }

        [JsonProperty("noball")]
        public int Noball { get; set; }

        [JsonProperty("econ")]
        public double Economy { get; set; }

        [JsonProperty("is_impact_player_in")]
        public int IsImpactPlayerIn { get; set; }

        [JsonProperty("is_impact_player_out")]
        public int IsImpactPlayerOut { get; set; }

        [JsonProperty("highlight_videos")]
        public List<string> HighlightVideos { get; set; }

        [JsonProperty("wicket_videos")]
        public List<string> WicketVideos { get; set; }

        [JsonProperty("short_name")]
        public string ShortName { get; set; }
    }
}
