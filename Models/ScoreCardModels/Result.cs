using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models.ScoreCardModels
{
    public class Result
    {
        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("is_draw")]
        public int IsDraw { get; set; }

        [JsonProperty("is_tie")]
        public int IsTie { get; set; }

        [JsonProperty("is_no_result")]
        public int IsNoResult { get; set; }
    }
}
