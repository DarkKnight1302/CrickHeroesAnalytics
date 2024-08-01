using Newtonsoft.Json;
using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models.ScoreCardModels
{
    public class ExtraData
    {
        [JsonProperty("extra_type_id")]
        public int ExtraTypeId { get; set; }

        [JsonProperty("type_code")]
        public string TypeCode { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("extra_type_run")]
        public int ExtraTypeRun { get; set; }

        [JsonProperty("extra_run")]
        public int ExtraRun { get; set; }
    }
}
