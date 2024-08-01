using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models
{
    public class FilterData
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }
    }
}
