using Newtonsoft.Json;

namespace CricHeroesAnalytics.Entities
{
    public class GroundSlot
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "uid")]
        public string ground { get; set; }

        public string GroundUrl { get; set; }

        public bool IsMorningSlotAvailable { get; set; } = false;

        public bool IsAvailable { get; set; } = false;

        public List<DateTime> AvailableDates { get; set;} = new List<DateTime>();

        public int DistanceInKm { get; set; }
    }
}
