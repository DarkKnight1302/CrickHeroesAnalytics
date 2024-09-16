using CricHeroesAnalytics.Entities;

namespace CricHeroesAnalytics.Repositories
{
    public interface IMatchRepository
    {
        public Task<bool> IsMatchAlreadyUpdated(string matchId);

        public Task AddMatch(Match match);

        public Task<List<Match>> GetAllMatches();
    }
}
