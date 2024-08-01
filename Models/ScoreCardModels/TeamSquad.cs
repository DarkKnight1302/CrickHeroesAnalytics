using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models.ScoreCardModels
{
    public class TeamSquad
    {
        [JsonProperty("data")]
        public List<SquadData> Data { get; set; }
    }
}
