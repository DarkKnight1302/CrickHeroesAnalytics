
using CricHeroesAnalytics.Constants;
using CricHeroesAnalytics.Entities;
using CricHeroesAnalytics.Extensions;
using CricHeroesAnalytics.Models.GWModels;
using CricHeroesAnalytics.Repositories;

namespace CricHeroesAnalytics.Services.Interfaces
{
    public class GwGroundAnalyticsService : IGwGroundAnalyticsService
    {
        private readonly ILogger _logger;
        private readonly IGWSportsApiClient gWSportsApiClient;
        private readonly IGroundSlotRepository groundSlotRepository;

        public GwGroundAnalyticsService(ILogger<IGwGroundAnalyticsService> logger, IGroundSlotRepository groundSlotRepository, IGWSportsApiClient gWSportsApiClient)
        {
            _logger = logger;
            this.groundSlotRepository = groundSlotRepository;
            this.gWSportsApiClient = gWSportsApiClient;
        }
        public async Task UpdateGroundSlots()
        {
            DateTimeOffset currentDateTime = DateTimeOffset.UtcNow.ToIndiaTime();
            await CleanUp(currentDateTime);
            for (int i = 4; i < 45; i++)
            {
                DateTimeOffset futureDate = currentDateTime.AddDays(i);
                if (futureDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    Dictionary<string, int> grounds = GroundList.grounds;
                    List<Task<(GroundSlots, string groundName)>> tasks = new List<Task<(GroundSlots, string groundName)>>();
                    foreach (var ground in grounds)
                    {
                        tasks.Add(Task.Run(() =>
                        {
                            return GetLatestSlots(ground.Key, futureDate);
                        }));
                    }
                    await Task.WhenAll(tasks);
                    foreach (var task in tasks)
                    {
                        (GroundSlots groundSlots, string groundName) = task.Result;
                        if (groundSlots != null && groundSlots.Status.Equals("success") && groundSlots.Data != null)
                        {
                            foreach (var slotData in groundSlots.Data)
                            {
                                if (slotData != null && !slotData.IsBooked && slotData.Rate <= 8500)
                                {
                                    GroundSlot dbSlot = await this.groundSlotRepository.GetGroundSlotAsync(groundName);
                                    if (dbSlot == null)
                                    {
                                        dbSlot = new GroundSlot()
                                        {
                                            Id = groundName,
                                            ground = groundName,
                                            GroundUrl = $"https://www.gwsportsapp.in/hyderabad/cricket/booking-sports-online-venue/{groundName}",
                                            DistanceInKm = GroundList.grounds[groundName],
                                            IsAvailable = true,
                                        };
                                    }
                                    dbSlot.AvailableDates.Add(futureDate);
                                    dbSlot.IsAvailable = true;
                                    await this.groundSlotRepository.UpdateGroundSlot(dbSlot);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private async Task CleanUp(DateTimeOffset currentDateTime)
        {
            List<GroundSlot> groundSlots = await this.groundSlotRepository.GetAllGroundSlotsAsync();
            foreach (var groundSlot in groundSlots)
            {
                if (groundSlot.IsAvailable)
                {
                    groundSlot.AvailableDates.RemoveAll(x => x < currentDateTime);
                    groundSlot.IsAvailable = groundSlot.AvailableDates.Count > 0;
                    await this.groundSlotRepository.UpdateGroundSlot(groundSlot);
                }
            }
        }

        private async Task<(GroundSlots, string groundName)> GetLatestSlots(string ground, DateTimeOffset futureDate)
        {
            var groundSlots = await this.gWSportsApiClient.GetGroundSlotsAsync(ground, futureDate);
            return (groundSlots, ground);
        }
    }
}
