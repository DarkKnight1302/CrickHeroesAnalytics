using Newtonsoft.Json;
using System;

namespace CricHeroesAnalytics.Models.ScoreCardModels
{
    public class SummaryData
    {
        public int match_id { get; set; }
        public string match_type { get; set; }
        public int is_super_over { get; set; }
        public string match_event_type { get; set; }
        public string match_event { get; set; }
        public int match_inning { get; set; }
        public int current_inning { get; set; }
        public int type { get; set; }
        public string ball_type { get; set; }
        public string status { get; set; }
        public int overs { get; set; }
        public int? balls { get; set; }
        public string over_reduce { get; set; }
        public int is_dl { get; set; }
        public int is_vjd { get; set; }
        public int ground_id { get; set; }
        public string ground_name { get; set; }
        public int city_id { get; set; }
        public string city_name { get; set; }
        public DateTime start_datetime { get; set; }
        public string win_by { get; set; }
        public string winning_team { get; set; }
        public string match_result { get; set; }
        public string tournament_id { get; set; }
        public string tournament_name { get; set; }
        public string tournament_category { get; set; }
        public string tournament_round_id { get; set; }
        public string tournament_round_name { get; set; }
        public string association_name { get; set; }
        public string association_logo { get; set; }
        public string steaming_url { get; set; }
        public int is_display_view { get; set; }
        public int is_ticker { get; set; }
        public int is_backend_match { get; set; }
        public int is_video_analyst { get; set; }
        public int is_in_review { get; set; }
        public int is_enable_tournament_streaming { get; set; }
        public int is_enable_match_streaming { get; set; }
        public Team team_a { get; set; }
        public Team team_b { get; set; }
        public List<string> insight_statements { get; set; }
        public List<object> streaming_details { get; set; }
        public int primary_video_seconds { get; set; }
        public string match_category_name { get; set; }
        public List<object> highlight_video { get; set; }
        public string toss_details { get; set; }
        public string streaming_multiple_url { get; set; }
        public string web_title_message { get; set; }
        public string short_url { get; set; }
        public MatchSummary match_summary { get; set; }
        public string share_message { get; set; }
        public string whatsapp_share_url { get; set; }
        public BestPerformances best_performances { get; set; }
        public PlayerOfTheMatch player_of_the_match { get; set; }
        public List<object> match_official { get; set; }
        public List<MatchNote> match_notes { get; set; }
        public List<object> scorer_notes { get; set; }
        public List<object> match_close_of_play { get; set; }
        public string tiny_share_url { get; set; }
    }

}
