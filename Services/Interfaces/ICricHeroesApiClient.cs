namespace CricHeroesAnalytics.Services.Interfaces
{
    public interface ICricHeroesApiClient
    {
        public Task GetMatches();

        public Task GetScoreCard();
    }
}
