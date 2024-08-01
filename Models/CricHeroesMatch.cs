using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models
{
    public class CricHeroesMatch
    {
        [JsonProperty("pageProps")]
        public PageProps PageProps { get; set; }

        [JsonProperty("__N_SSP")]
        public bool NSSP { get; set; }
    }
}
