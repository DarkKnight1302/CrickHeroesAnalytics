using CricHeroesAnalytics.Components;
using CricHeroesAnalytics.CronJob;
using CricHeroesAnalytics.Repositories;
using CricHeroesAnalytics.Services;
using CricHeroesAnalytics.Services.Interfaces;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<ICricHeroesApiClient, CricHeroesApiClient>();
builder.Services.AddSingleton<IMatchAnalyticService, MatchAnalyticService>();
builder.Services.AddSingleton<ICosmosDbService, CosmosDbService>();
builder.Services.AddSingleton<ISecretService, SecretService>();
builder.Services.AddSingleton<IPlayerAnalyticsService, PlayerAnalyticsService>();
builder.Services.AddSingleton<IMatchRepository, MatchRepository>();
builder.Services.AddSingleton<IPlayerRepository, PlayerRepository>();
builder.Services.AddSingleton<IJobExecutionRepository, JobExecutionRespository>();
builder.Services.AddSingleton<IGroundSlotRepository, GroundSlotRepository>();
builder.Services.AddSingleton<IGwGroundAnalyticsService, GwGroundAnalyticsService>();
builder.Services.AddSingleton<IGWSportsApiClient, GWSportsApiClient>();
builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("ScoreUpdateJob");

    q.AddJob<ScoreUpdateJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
                        .ForJob(jobKey)
                        .WithIdentity("ScoreUpdateJob-trigger")
                        .WithCronSchedule("0 0 */3 * * ?"));
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
