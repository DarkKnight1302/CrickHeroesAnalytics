using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models
{
    public class Config
    {
        [JsonProperty("match_page_size")]
        public int MatchPageSize { get; set; }

        [JsonProperty("page_title")]
        public string PageTitle { get; set; }

        [JsonProperty("share_message")]
        public string ShareMessage { get; set; }

        [JsonProperty("total_count")]
        public int TotalCount { get; set; }
    }
}
