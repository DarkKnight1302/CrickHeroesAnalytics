using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models.ScoreCardModels
{
    public class PageProps
    {
        [JsonProperty("tab")]
        public string Tab { get; set; }

        [JsonProperty("miniScorecard")]
        public MiniScorecard MiniScorecard { get; set; }

        [JsonProperty("scorecard")]
        public List<Scorecard> Scorecard { get; set; }

        [JsonProperty("mvp")]
        public Mvp Mvp { get; set; }

        [JsonProperty("summaryData")]
        public SummaryDataExternal SummaryData { get; set; }

        [JsonProperty("teamSquad")]
        public TeamSquad TeamSquad { get; set; }

        [JsonProperty("upcomingResponse")]
        public UpcomingResponse UpcomingResponse { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("userAgent")]
        public string UserAgent { get; set; }
    }
}
