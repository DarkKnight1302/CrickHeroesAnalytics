using Newtonsoft.Json;

namespace CricHeroesAnalytics.Entities
{
    public class Player
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "uid")]
        public string  Uid { get; set; }
        public string Name { get; set; }

        public int TotalRuns { get; set; }

        public int TotalWickets { get; set; }

        public int MatchesPlayed { get; set; }

        public Dictionary<string, PlayerRunsPerMatch> PlayerRunMatchMap { get; set; } = new Dictionary<string, PlayerRunsPerMatch>();

        public Dictionary<string, PlayerWicketsPerMatch> PlayerWicketsMatchMap { get; set; } = new Dictionary<string, PlayerWicketsPerMatch>();
    }
}
