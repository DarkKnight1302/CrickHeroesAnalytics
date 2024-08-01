using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models.ScoreCardModels
{
    public class Player
    {
        [JsonProperty("player_id")]
        public int PlayerId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_impact_substitute")]
        public int IsImpactSubstitute { get; set; }

        [JsonProperty("is_impact_player_in")]
        public int IsImpactPlayerIn { get; set; }

        [JsonProperty("is_impact_player_out")]
        public int IsImpactPlayerOut { get; set; }

        [JsonProperty("short_name")]
        public string ShortName { get; set; }
    }
}
