using Newtonsoft.Json;

namespace CricHeroesAnalytics.Models
{
    public class MemberNudgeInfo
    {
        [JsonProperty("redirection_url")]
        public string RedirectionUrl { get; set; }

        [JsonProperty("redirection_type")]
        public string RedirectionType { get; set; }

        [JsonProperty("redirection_id")]
        public string RedirectionId { get; set; }

        [JsonProperty("is_external_link")]
        public int IsExternalLink { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("button_text")]
        public string ButtonText { get; set; }

        [JsonProperty("background_color")]
        public string BackgroundColor { get; set; }

        [JsonProperty("title_text_color")]
        public string TitleTextColor { get; set; }

        [JsonProperty("button_text_color")]
        public string ButtonTextColor { get; set; }
    }
}
