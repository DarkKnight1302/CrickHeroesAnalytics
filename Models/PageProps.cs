using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models
{
    public class PageProps
    {
        [JsonProperty("teamDetails")]
        public TeamDetails TeamDetails { get; set; }

        [JsonProperty("members")]
        public Members Members { get; set; }

        [JsonProperty("matches")]
        public Matches Matches { get; set; }

        [JsonProperty("teamStats")]
        public TeamStats TeamStats { get; set; }

        [JsonProperty("filters")]
        public Filters Filters { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("userAgent")]
        public string UserAgent { get; set; }

        [JsonProperty("tab")]
        public string Tab { get; set; }
    }

}
