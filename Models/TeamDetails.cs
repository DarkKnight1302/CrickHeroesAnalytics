using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models
{
    public class TeamDetails
    {
        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("data")]
        public TeamData Data { get; set; }
    }
}
