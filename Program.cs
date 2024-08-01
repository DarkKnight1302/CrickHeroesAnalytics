using CricHeroesAnalytics.Components;
using CricHeroesAnalytics.Repositories;
using CricHeroesAnalytics.Services;
using CricHeroesAnalytics.Services.Interfaces;

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
