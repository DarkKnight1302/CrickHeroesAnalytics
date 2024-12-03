using CricHeroesAnalytics.Models.GWModels;
using CricHeroesAnalytics.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.IO.Compression;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Web;

namespace CricHeroesAnalytics.Services
{
    public class GWSportsApiClient : IGWSportsApiClient
    {
        private const string GwUrl = "https://www.gwsportsapp.in/ajax-handler?t=gsearch&action=getSlotsForGroundSport";
        private readonly ILogger logger;
        private readonly IMemoryCache memoryCache;

        public GWSportsApiClient(ILogger<IGWSportsApiClient> logger, IMemoryCache memoryCache) 
        {
            this.logger = logger;
            this.memoryCache = memoryCache;
        }

        public async Task<GroundSlots> GetGroundSlotsAsync(string ground, DateTimeOffset dateTime)
        {
            try
            {
                this.logger.LogInformation("Calling Get Ground Slots");
                string formatedDate = dateTime.ToString("yyyy-MM-dd");
                HttpClient client = this.GetHttpClient(ground);
                var obj = new { l = "hyderabad", g = ground, s = "cricket", d = formatedDate };
                string value = JsonConvert.SerializeObject(obj);
                string encoded = HttpUtility.UrlEncode(value);
                var content = new StringContent($"data={encoded}");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded") { CharSet = "UTF-8" };

                // Send the POST request
                HttpResponseMessage response = await client.PostAsync(GwUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidDataException($"Non Success status code from GW {response.StatusCode}");
                }
                string responseString = string.Empty;
                using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                {
                    Stream decompressionStream = null;
                    if (response.Content.Headers.ContentEncoding.Contains("gzip"))
                    {
                        decompressionStream = new GZipStream(responseStream, CompressionMode.Decompress);
                    }
                    else if (response.Content.Headers.ContentEncoding.Contains("deflate"))
                    {
                        decompressionStream = new DeflateStream(responseStream, CompressionMode.Decompress);
                    }
                    else if (response.Content.Headers.ContentEncoding.Contains("br"))
                    {
                        decompressionStream = new BrotliStream(responseStream, CompressionMode.Decompress);
                    }
                    else
                    {
                        decompressionStream = responseStream; // No compression
                    }

                    using (decompressionStream)
                    using (StreamReader reader = new StreamReader(decompressionStream))
                    {
                        responseString = await reader.ReadToEndAsync();
                        this.logger.LogInformation($"Slot details for ground {ground} : {responseString}");
                    }
                }

                return JsonConvert.DeserializeObject<GroundSlots>(responseString);
            } catch (Exception e)
            {
                this.logger.LogError($"Exception Calling Get Ground Slots {ground} : {e.Message}");
                return null;
            }
        }

        private HttpClient GetHttpClient(string ground)
        {
            string key = $"{ground}_httpClient";
            if (this.memoryCache.TryGetValue<HttpClient>(key, out HttpClient httpClient))
            {
                return httpClient;
            }
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("br"));
            client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US", 0.9));
            client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-IN", 0.8));
            client.DefaultRequestHeaders.Add("Origin", "https://www.gwsportsapp.in");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            client.DefaultRequestHeaders.Add("sec-ch-ua", "\"Not/A)Brand\";v=\"8\", \"Chromium\";v=\"126\", \"Microsoft Edge\";v=\"126\"");
            client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            client.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            client.DefaultRequestHeaders.Referrer = new Uri($"https://www.gwsportsapp.in/hyderabad/cricket/booking-sports-online-venue/{ground}");
            this.memoryCache.Set<HttpClient>(key, client, DateTimeOffset.Now.AddHours(3d));
            return client;
        }
    }
}
