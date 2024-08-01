using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models
{
    public class Page
    {
        [JsonProperty("current_page")]
        public int CurrentPage { get; set; }

        [JsonProperty("per_page")]
        public int PerPage { get; set; }

        [JsonProperty("total_page")]
        public int TotalPage { get; set; }

        [JsonProperty("total_count")]
        public int TotalCount { get; set; }
    }
}
