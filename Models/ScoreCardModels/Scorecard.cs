using System.Numerics;
using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models.ScoreCardModels
{
    public class Scorecard
    {
        [JsonProperty("team_id")]
        public int TeamId { get; set; }

        [JsonProperty("inning")]
        public Inning Inning { get; set; }

        [JsonProperty("batting")]
        public List<Batting> Batting { get; set; }

        [JsonProperty("extras")]
        public Extras Extras { get; set; }

        [JsonProperty("to_be_bat")]
        public List<Player> ToBeBat { get; set; }

        [JsonProperty("fall_of_wicket")]
        public FallOfWicket FallOfWicket { get; set; }

        [JsonProperty("bowling")]
        public List<Bowling> Bowling { get; set; }

        [JsonProperty("teamName")]
        public string TeamName { get; set; }
    }

}
