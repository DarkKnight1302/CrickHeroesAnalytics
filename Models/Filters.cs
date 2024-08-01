using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models
{
    public class Filters
    {
        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("data")]
        public List<FilterData> Data { get; set; }
    }
}
