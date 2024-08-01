using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models.ScoreCardModels
{
    public class Venue
    {
        [JsonProperty("venue_id")]
        public int VenueId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
