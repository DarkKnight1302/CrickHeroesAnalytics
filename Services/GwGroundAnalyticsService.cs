﻿using CricHeroesAnalytics.Constants;
using CricHeroesAnalytics.Entities;
using CricHeroesAnalytics.Extensions;
using CricHeroesAnalytics.Models.GWModels;
using CricHeroesAnalytics.Repositories;
using CricHeroesAnalytics.Services.Interfaces;

namespace CricHeroesAnalytics.Services
{
    public class GwGroundAnalyticsService : IGwGroundAnalyticsService
    {
        private readonly ILogger _logger;
        private readonly IGWSportsApiClient gWSportsApiClient;
        private readonly IGroundSlotRepository groundSlotRepository;
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public GwGroundAnalyticsService(ILogger<IGwGroundAnalyticsService> logger, IGroundSlotRepository groundSlotRepository, IGWSportsApiClient gWSportsApiClient)
        {
            _logger = logger;
            this.groundSlotRepository = groundSlotRepository;
            this.gWSportsApiClient = gWSportsApiClient;
        }
        public async Task UpdateGroundSlots()
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                DateTimeOffset currentDateTime = DateTimeOffset.UtcNow.ToIndiaTime();
                await CleanUp(currentDateTime);
                for (int i = 4; i < 30; i++)
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
                            if (groundSlots == null || !groundSlots.Status.Equals("success"))
                            {
                                this._logger.LogError($"No slots found for ground {groundName}");
                            }
                            if (groundSlots != null && groundSlots.Status.Equals("success") && groundSlots.Data != null)
                            {
                                bool groundAvailable = false;
                                bool isMorningSlotAvailable = false;
                                foreach (var slotData in groundSlots.Data)
                                {
                                    if (slotData != null && !slotData.IsBooked && slotData.Rate <= 14000 && slotData.SlotTimeHalf <= 700 && slotData.SlotTimeHalf > 300)
                                    {
                                        groundAvailable = true;
                                    }
                                    if (slotData != null && slotData.SlotTimeHalf < 500 && slotData.SlotTimeHalf > 400 && !slotData.IsBooked)
                                    {
                                        isMorningSlotAvailable = true;
                                    }
                                }

                                GroundSlot dbSlot = await groundSlotRepository.GetGroundSlotAsync(groundName);
                                if (dbSlot == null)
                                {
                                    dbSlot = new GroundSlot()
                                    {
                                        Id = groundName,
                                        ground = groundName,
                                        GroundUrl = $"https://www.gwsportsapp.in/hyderabad/cricket/booking-sports-online-venue/{groundName}",
                                        DistanceInKm = GroundList.grounds[groundName],
                                        IsAvailable = groundAvailable,
                                    };
                                }

                                dbSlot.DistanceInKm = GroundList.grounds[groundName];

                                if (groundAvailable)
                                {
                                    if (!dbSlot.AvailableDates.Contains(futureDate.Date))
                                    {
                                        dbSlot.AvailableDates.Add(futureDate.Date);
                                        dbSlot.IsAvailable = true;
                                    }
                                    dbSlot.IsMorningSlotAvailable = isMorningSlotAvailable;
                                }
                                else
                                {
                                    dbSlot.AvailableDates.Remove(futureDate.Date);
                                    dbSlot.IsAvailable = dbSlot.AvailableDates.Count > 0;
                                }
                                await groundSlotRepository.UpdateGroundSlot(dbSlot);
                            }
                        }
                    }
                }
            } finally
            {
                this.semaphoreSlim.Release();
            }
        }

        private async Task CleanUp(DateTimeOffset currentDateTime)
        {
            List<GroundSlot> groundSlots = await groundSlotRepository.GetAllGroundSlotsAsync();
            foreach (var groundSlot in groundSlots)
            {
                if (groundSlot.IsAvailable)
                {
                    groundSlot.AvailableDates.RemoveAll(x => x < currentDateTime);
                    groundSlot.IsAvailable = groundSlot.AvailableDates.Count > 0;
                    await groundSlotRepository.UpdateGroundSlot(groundSlot);
                }
            }
        }

        private async Task<(GroundSlots, string groundName)> GetLatestSlots(string ground, DateTimeOffset futureDate)
        {
            var groundSlots = await gWSportsApiClient.GetGroundSlotsAsync(ground, futureDate);
            return (groundSlots, ground);
        }
    }
}
