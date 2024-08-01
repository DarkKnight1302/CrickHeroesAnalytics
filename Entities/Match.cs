using CricHeroesAnalytics.Models.ScoreCardModels;
using Newtonsoft.Json;

namespace CricHeroesAnalytics.Entities
{
    public class Match
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "matchId")]
        public string MatchId { get; set; }

        public List<Batting> Batting { get; set; }

        public List<Bowling> Bowling { get; set; }
    }
}
