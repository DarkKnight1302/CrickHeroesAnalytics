using CricHeroesAnalytics.Models;
using CricHeroesAnalytics.Models.ScoreCardModels;
using CricHeroesAnalytics.Services.Interfaces;
using CricHeroesAnalytics.Utilities;
using System.Net.Http.Headers;
using System.Security.Policy;

namespace CricHeroesAnalytics.Services
{
    public class CricHeroesApiClient : ICricHeroesApiClient
    {
        private readonly ILogger logger;
        private string buildId;

        public CricHeroesApiClient(ILogger<CricHeroesApiClient> logger)
        {
            this.logger = logger;
        }
        public async Task<List<MatchData>> GetMatches()
        {
            try
            {
                await UpdateBuildId();
                using (HttpClient client = new HttpClient())
                {
                    // Set base address
                    client.BaseAddress = new Uri("https://cricheroes.com");

                    // Set headers
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                    client.DefaultRequestHeaders.Add("accept-language", "en-US,en;q=0.9,en-IN;q=0.8");
                    client.DefaultRequestHeaders.Add("priority", "u=1, i");
                    client.DefaultRequestHeaders.Add("referer", "https://cricheroes.com/team-profile/5455774/cult-100/members");
                    client.DefaultRequestHeaders.Add("sec-ch-ua", "\"Not)A;Brand\";v=\"99\", \"Microsoft Edge\";v=\"127\", \"Chromium\";v=\"127\"");
                    client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
                    client.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
                    client.DefaultRequestHeaders.Add("sec-fetch-dest", "empty");
                    client.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
                    client.DefaultRequestHeaders.Add("sec-fetch-site", "same-origin");
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/127.0.0.0 Safari/537.36 Edg/127.0.0.0");
                    client.DefaultRequestHeaders.Add("x-nextjs-data", "1");

                    // Make the GET request
                    HttpResponseMessage response = await client.GetAsync($"/_next/data/{this.buildId}/team-profile/5455774/cult-100/matches.json?teamId=5455774&teamName=cult-100&tabName=matches");
                    response.EnsureSuccessStatusCode();

                    string matchResponse = await response.Content.ReadAsStringAsync();
                    CricHeroesMatch cricHeroesMatch = JsonUtil.DeSerialize<CricHeroesMatch>(matchResponse);
                    this.logger.LogInformation(JsonUtil.SerializeObject(cricHeroesMatch));
                    return cricHeroesMatch?.PageProps?.Matches?.Data ?? null;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message + "\n" + ex.StackTrace);
                return null;
            }
        }

        public async Task<ScoreCardResponse> GetScoreCard(MatchData matchData)
        {
            try
            {
                if (matchData == null)
                {
                    return null;
                }
                string combinedTeamName = GenerateTeamName(matchData.TeamA, matchData.TeamB);
                long matchId = matchData.MatchId;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                    client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US"));
                    client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en", 0.9));
                    client.DefaultRequestHeaders.Add("priority", "u=1, i");
                    client.DefaultRequestHeaders.Referrer = new Uri($"https://cricheroes.com/scorecard/{matchId}/individual/{combinedTeamName}/summary");
                    client.DefaultRequestHeaders.Add("sec-ch-ua", "\"Not)A;Brand\";v=\"99\", \"Microsoft Edge\";v=\"127\", \"Chromium\";v=\"127\"");
                    client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
                    client.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
                    client.DefaultRequestHeaders.Add("sec-fetch-dest", "empty");
                    client.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
                    client.DefaultRequestHeaders.Add("sec-fetch-site", "same-origin");
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/127.0.0.0 Safari/537.36 Edg/127.0.0.0");
                    client.DefaultRequestHeaders.Add("x-nextjs-data", "1");

                    HttpResponseMessage response = await client.GetAsync($"https://cricheroes.com/_next/data/{this.buildId}/scorecard/{matchId}/individual/{combinedTeamName}/scorecard.json");
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    this.logger.LogInformation(JsonUtil.SerializeObject(responseBody));
                    ScoreCardResponse scoreCardResponse = JsonUtil.DeSerialize<ScoreCardResponse>(responseBody);
                    this.logger.LogInformation(JsonUtil.SerializeObject(scoreCardResponse));
                    return scoreCardResponse;
                }
            } catch (Exception ex)
            {
                this.logger.LogError($"{ex.Message} \n {ex.StackTrace}");
                return null;
            }
        }

        private async Task UpdateBuildId()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    string htmlContent = await client.GetStringAsync("https://cricheroes.com/team-profile/5455774/cult-100/matches");
                    if (htmlContent != null)
                    {
                        int index = htmlContent.IndexOf("/_buildManifest.js");
                        if (index < 0)
                        {
                            return;
                        }
                        int endIndex = index;
                        index--;
                        while (htmlContent[index] != '/')
                        {
                            index--;
                        }
                        int startIndex = index + 1;
                        this.buildId = htmlContent.Substring(startIndex, endIndex - startIndex);
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError($"An error occurred: {ex.Message}");
                }
            }
        }

        private string GenerateTeamName(string teamA, string teamB)
        {
            teamA = teamA.ToLower();
            teamB = teamB.ToLower();
            teamA = teamA.Replace(" ", "-");
            teamB = teamB.Replace(" ", "-");
            return $"{teamA}-vs-{teamB}";
        }
    }
}
