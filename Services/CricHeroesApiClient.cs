using CricHeroesAnalytics.Models;
using CricHeroesAnalytics.Services.Interfaces;
using CricHeroesAnalytics.Utilities;
using System.Net.Http.Headers;

namespace CricHeroesAnalytics.Services
{
    public class CricHeroesApiClient : ICricHeroesApiClient
    {
        private readonly ILogger logger;
        public CricHeroesApiClient(ILogger<CricHeroesApiClient> logger)
        {
            this.logger = logger;
        }
        public async Task GetMatches()
        {
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
                HttpResponseMessage response = await client.GetAsync("/_next/data/n6cDqXt9q0M2Y0HFFt4u7/team-profile/5455774/cult-100/matches.json?teamId=5455774&teamName=cult-100&tabName=matches");

                if (response.IsSuccessStatusCode)
                {
                    /*CricHeroesMatch cricHeroesMatch = await response.Content.ReadFromJsonAsync<CricHeroesMatch>().ConfigureAwait(false);
                    this.logger.LogInformation(JsonUtil.SerializeObject(cricHeroesMatch));*/
                    string matchResponse = await response.Content.ReadAsStringAsync();
                    CricHeroesMatch cricHeroesMatch = JsonUtil.DeSerialize<CricHeroesMatch>(matchResponse);
                    this.logger.LogInformation(JsonUtil.SerializeObject(cricHeroesMatch));
                }
                else
                {
                    this.logger.LogInformation("Error: " + response.StatusCode);
                }
            }
        }

        public Task GetScoreCard()
        {
            throw new NotImplementedException();
        }
    }
}
