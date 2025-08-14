using CricHeroesAnalytics.Models;
using CricHeroesAnalytics.Models.ScoreCardModels;
using CricHeroesAnalytics.Services.Interfaces;
using CricHeroesAnalytics.Utilities;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using PuppeteerSharp;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Net.Http;
using System.Text.Json;
using HtmlAgilityPack;
using System.Collections.Concurrent;

namespace CricHeroesAnalytics.Services
{
    public class CricHeroesApiClient : ICricHeroesApiClient
    {
        private static readonly ConcurrentDictionary<string, string?> _profileUrlCache = new();
        private readonly ILogger logger;
        private string buildId;
        private const string BuildConstant = "76g_qHZmcMiNzqsUhmXqh";
        private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private HttpClient _httpClient = new HttpClient();

        public CricHeroesApiClient(ILogger<CricHeroesApiClient> logger)
        {
            buildId = BuildConstant;
            this.logger = logger;
        }

        public async Task<List<MatchData>> GetMatches()
        {
            await UpdateBuildIdAsync();
            this.logger.LogInformation("Request to fetch matches");
            await semaphore.WaitAsync();
            try
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
                    HttpResponseMessage response = await client.GetAsync($"/_next/data/{this.buildId}/team-profile/5455774/cult-100/matches.json?teamId=5455774&teamName=cult-100&tabName=matches");
                    response.EnsureSuccessStatusCode();

                    string matchResponse = await response.Content.ReadAsStringAsync();
                    CricHeroesMatch cricHeroesMatch = JsonUtil.DeSerialize<CricHeroesMatch>(matchResponse);
                    this.logger.LogInformation(JsonUtil.SerializeObject(cricHeroesMatch));
                    return cricHeroesMatch?.PageProps?.Matches?.Data ?? null;
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task<ScoreCardResponse> GetScoreCard(MatchData matchData)
        {
            if (matchData == null)
            {
                throw new InvalidDataException("Invalid Match data");
            }
            await UpdateBuildIdAsync();
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

        private async Task UpdateBuildIdAsync()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync("https://cricheroes.com/team-profile/5455774/cult-100/matches");
                if (!string.IsNullOrEmpty(response))
                {
                    int index = response.IndexOf("/_buildManifest.js");
                    if (index > 0)
                    {
                        int endIndex = index;
                        index--;
                        while (response[index] != '/')
                        {
                            index--;
                        }
                        int startIndex = index + 1;
                        string buildId = response.Substring(startIndex, endIndex - startIndex);
                        this.buildId = buildId;
                    }
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

        public void UpdateBuildId(string buildIdLatest)
        {
            this.buildId = buildIdLatest;
        }

        public async Task<string?> GetProfilePictureUrlAsync(string profileId)
        {
            if (string.IsNullOrWhiteSpace(profileId))
            {
                return null;
            }

            // --- CACHING STEP 1: Check the cache first ---
            if (_profileUrlCache.TryGetValue(profileId, out var cachedUrl))
            {
                System.Diagnostics.Debug.WriteLine($"Cache HIT for profile ID: {profileId}");
                return cachedUrl; // Return the cached URL immediately
            }

            // --- If not in cache, proceed to fetch ---
            System.Diagnostics.Debug.WriteLine($"Cache MISS for profile ID: {profileId}. Fetching from web.");
            var fetchedUrl = await FetchAndParseUrlFromWebAsync(profileId);

            // --- CACHING STEP 2: Add the newly fetched URL to the cache ---
            // We use TryAdd to handle the rare case where another request might have added it
            // just after our cache check.
            if (!string.IsNullOrEmpty(fetchedUrl))
            {
                _profileUrlCache.TryAdd(profileId, fetchedUrl);
            }
            
            return fetchedUrl;
        }

        /// <summary>
        /// Contains the actual logic to scrape the webpage for the URL.
        /// </summary>
        private async Task<string?> FetchAndParseUrlFromWebAsync(string profileId)
        {
            var requestUri = $"https://cricheroes.com/player-profile/{profileId}/stats/matches";

            try
            {
                var htmlContent = await _httpClient.GetStringAsync(requestUri);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlContent);
                var scriptNode = htmlDoc.GetElementbyId("__NEXT_DATA__");

                if (scriptNode == null) return null;

                var jsonData = scriptNode.InnerHtml;
                using var jsonDoc = JsonDocument.Parse(jsonData);
                return FindJsonProperty(jsonDoc.RootElement, "profile_photo");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"An error occurred while fetching profile {profileId}: {ex.Message}");
                return "https://media.cricheroes.in/default/user_profile.png";
            }
        }

        /// <summary>
        /// Recursively searches a JsonElement for a specific property name.
        /// </summary>
        private string? FindJsonProperty(JsonElement element, string propertyName)
        {
            if (element.ValueKind == JsonValueKind.Object)
            {
                if (element.TryGetProperty(propertyName, out var propertyValue) && propertyValue.ValueKind == JsonValueKind.String)
                {
                    return propertyValue.GetString();
                }

                foreach (var property in element.EnumerateObject())
                {
                    var result = FindJsonProperty(property.Value, propertyName);
                    if (result != null) return result;
                }
            }
            else if (element.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in element.EnumerateArray())
                {
                    var result = FindJsonProperty(item, propertyName);
                    if (result != null) return result;
                }
            }

            return null;
        }

        public void ResetCache()
        {
            _profileUrlCache.Clear();
        }
    }
}

