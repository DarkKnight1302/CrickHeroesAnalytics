using Newtonsoft.Json;
using System.Drawing.Drawing2D;

namespace CricHeroesAnalytics.Models
{
    public class Matches
    {
        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("page")]
        public Page Page { get; set; }

        [JsonProperty("data")]
        public List<MatchData> Data { get; set; }

        [JsonProperty("config")]
        public Config Config { get; set; }
    }
}
