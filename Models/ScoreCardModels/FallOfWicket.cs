using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models.ScoreCardModels
{
    public class FallOfWicket
    {
        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("data")]
        public List<FallOfWicketData> Data { get; set; }
    }
}
