using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models.ScoreCardModels
{
    public class Extras
    {
        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("data")]
        public List<ExtraData> Data { get; set; }
    }
}
