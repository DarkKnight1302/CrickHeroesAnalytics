using Newtonsoft.Json;
using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models.ScoreCardModels
{
    public class ScoreCardResponse
    {
        [JsonProperty("__N_SSP")]
        public bool N_SSP { get; set; }

        [JsonProperty("pageProps")]
        public PageProps PageProps { get; set; }
    }
}
