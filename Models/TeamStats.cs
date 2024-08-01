using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models
{
    public class TeamStats
    {
        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("data")]
        public Dictionary<string, object> Data { get; set; }
    }
}
