using CricHeroesAnalytics.Models;
using CricHeroesAnalytics.Models.ScoreCardModels;
using CricHeroesAnalytics.Services.Interfaces;
using CricHeroesAnalytics.Utilities;
using PuppeteerSharp;
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
            BrowserFetcher browserFetcher = new BrowserFetcher();
            _ = browserFetcher.DownloadAsync().Result;
        }
        public async Task<List<MatchData>> GetMatches()
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

        public async Task<ScoreCardResponse> GetScoreCard(MatchData matchData)
        {
            if (matchData == null)
            {
                throw new InvalidDataException("Invalid Match data");
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
                if (scoreCardResponse == null
                    || scoreCardResponse.PageProps.Scorecard.Count == 0
                    || scoreCardResponse.PageProps.Scorecard[0].Batting == null
                    || scoreCardResponse.PageProps.Scorecard[0].Batting.Count == 0)
                {
                    this.logger.LogError("Invalid scorecard response");
                    throw new InvalidDataException("Invalid Scorecard response");
                }
                return scoreCardResponse;
            }
        }

        private async Task UpdateBuildId()
        {
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false // Set to false if you want to see the browser window
            });

            try
            {

                // Create a new page
                var page = await browser.NewPageAsync();

                // Set the necessary headers
                //            await page.SetExtraHttpHeadersAsync(new System.Collections.Generic.Dictionary<string, string>
                //{
                //    { "accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7" },
                //    { "accept-language", "en-US,en;q=0.9" },
                //    { "cache-control", "max-age=0" },
                //    { "priority", "u=0, i" },
                //    { "sec-ch-ua", "\"Chromium\";v=\"128\", \"Not;A=Brand\";v=\"24\", \"Microsoft Edge\";v=\"128\"" },
                //    { "sec-ch-ua-mobile", "?0" },
                //    { "sec-ch-ua-platform", "\"Windows\"" },
                //    { "sec-fetch-dest", "document" },
                //    { "sec-fetch-mode", "navigate" },
                //    { "sec-fetch-site", "same-origin" },
                //    { "sec-fetch-user", "?1" },
                //    { "upgrade-insecure-requests", "1" },
                //    { "user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/128.0.0.0 Safari/537.36 Edg/128.0.0.0" }
                //});

                // Set the cookies
                var cookies = new CookieParam[]
                {
            new CookieParam { Name = "udid", Value = "8096b70bf47c5b69247d60a1239d90aa", Domain = "cricheroes.com" },
            new CookieParam { Name = "_fbp", Value = "fb.1.1725695411476.374579704170436648", Domain = "cricheroes.com" },
            new CookieParam { Name = "_gcl_au", Value = "1.1.915983878.1725695412", Domain = "cricheroes.com" },
            new CookieParam { Name = "_ga", Value = "GA1.1.1795357793.1725695412", Domain = "cricheroes.com" },
            new CookieParam { Name = "_cc_id", Value = "8a688e0c693bf9df657a7c9fedc6fe1f", Domain = "cricheroes.com" },
            new CookieParam { Name = "panoramaId_expiry", Value = "1725781811448", Domain = "cricheroes.com" },
            new CookieParam { Name = "_clck", Value = "2kldq0%7C2%7Cfoz%7C0%7C1711", Domain = "cricheroes.com" },
            new CookieParam { Name = "cto_bundle", Value = "LybzfV9mTG5CYmlaRms3UVo3ejZTZXlmN3JKQXFBbE43NFpmY1Fob1NjWUF6Y1dQN0VlOXR4WWdtMlVzZDZ3b2FBNUtUaWdqaFJGbElETW5DVFVzZmEwejhaTUFvY29PU2o4b1VtSGpiYVhmdmhQUnZka1cwQ3YxMzhObDVITFBPTERibk94RWdtOCUyRm5oWTE2WTNDNXJYbTBpUSUzRCUzRA", Domain = "cricheroes.com" },
            new CookieParam { Name = "FCNEC", Value = "%5B%5B%22AKsRol8KWX5Vxoi7nVXx6SOTnwK_zkAIvmPNa57SIsSvPKHCpUXN_SJD7IJE_VxMIj6EJ76qVekw7kspkZNRUrJnzKXiYvuviaQFvt4l8IPZiXnPS7w0ylM0KsCXRx4Xyetza97ZYYuRc4XAKnpjSxZByMFIRoQxIQ%3D%3D%22%5D%5D", Domain = "cricheroes.com" },
            new CookieParam { Name = "_ga_RHRT76MSXD", Value = "GS1.1.1725695411.1.1.1725695722.60.0.0", Domain = "cricheroes.com" },
            new CookieParam { Name = "_clsk", Value = "16tl17z%7C1725698192501%7C1%7C1%7Cz.clarity.ms%2Fcollect", Domain = "cricheroes.com" }
                };

                //await page.SetCookieAsync(cookies);

                // Navigate to the URL
                await page.GoToAsync("https://cricheroes.com/team-profile/5455774/cult-100/matches");

                // Optionally, wait for a response if needed
                //var response = await page.WaitForResponseAsync(response => response.Url.Contains("matches") && response.Status == System.Net.HttpStatusCode.OK);
                //Console.WriteLine($"Response Status: {response.Status}");
                await Task.Delay(3000);
                // Get the content of the page
                var htmlContent = await page.GetContentAsync();
                this.logger.LogInformation(htmlContent);

                // Close browser
                await browser.CloseAsync();

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
            } finally
            {
                await browser.CloseAsync();
                browser.Dispose();
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
