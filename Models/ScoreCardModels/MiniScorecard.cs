using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models.ScoreCardModels
{
    public class MiniScorecard
    {
        [JsonProperty("status")]
        public bool Status { get; set; }
    }
}
