using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models.ScoreCardModels
{
    public class Summary
    {
        [JsonProperty("score")]
        public string Score { get; set; }

        [JsonProperty("over")]
        public string Over { get; set; }

        [JsonProperty("ball")]
        public string Ball { get; set; }

        [JsonProperty("rr")]
        public string Rr { get; set; }
    }
}
