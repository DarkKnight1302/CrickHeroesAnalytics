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

        public string PlayerProfile => $"https://cricheroes.com/player-profile/{this.Id}/stats/stats";

        public double BattingAverage => Math.Round(this.GotOutCount > 0 ? (double)this.TotalRuns / (double)this.GotOutCount : 0D, 1);

        public int GotOutCount { get; set; } = 0;

        public int BallsPlayed { get; set; }

        public int StrikeRate { get; set; } = 0;

        public int TotalWickets { get; set; }

        public int RunsGiven { get; set; }

        public double OversBowled { get; set; }

        public double BowlingEconomy { get; set; }

        public int MatchesPlayed { get; set; }

        public double BowlingAverage { get; set; }

        public DateTimeOffset LastMatchUpdated { get; set; } = DateTimeOffset.MinValue;

        public Dictionary<string, PlayerRunsPerMatch> PlayerRunMatchMap { get; set; } = new Dictionary<string, PlayerRunsPerMatch>();

        public Dictionary<string, PlayerWicketsPerMatch> PlayerWicketsMatchMap { get; set; } = new Dictionary<string, PlayerWicketsPerMatch>();
    }
}
