using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models
{
    public class Members
    {
        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("data")]
        public Dictionary<string, object> Data { get; set; }

        [JsonProperty("page")]
        public Page Page { get; set; }
    }
}
