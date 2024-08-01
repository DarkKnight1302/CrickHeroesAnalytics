using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models.ScoreCardModels
{
    public class FallOfWicketData
    {
        [JsonProperty("run")]
        public int Run { get; set; }

        [JsonProperty("wicket")]
        public int Wicket { get; set; }

        [JsonProperty("over")]
        public double Over { get; set; }

        [JsonProperty("balls")]
        public int Balls { get; set; }

        [JsonProperty("dismiss_player_id")]
        public int DismissPlayerId { get; set; }

        [JsonProperty("dismiss_player_name")]
        public string DismissPlayerName { get; set; }
    }
}
