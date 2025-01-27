using Microsoft.EntityFrameworkCore;
using TradingApp.Application.Services.Interfaces.Database;
using TradingApp.Domain.Spot;

namespace TradingApp.Application.Repositories.SpotPortfolioRepository
{
    public class SpotPortfolioRepository : ISpotPortfolioRepository
    {
        private readonly IDbContext _context;

        public SpotPortfolioRepository(IDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SpotPortfolio>> GetSpotPortfolios()
        {
            var spotPortfolios = _context.Set<SpotPortfolio>().AsEnumerable();
            return spotPortfolios;
        }

        public async Task<SpotPortfolio> GetSpotPortfolioById(int id)
        {
            var spotPortfolio = await _context.Set<SpotPortfolio>().FirstOrDefaultAsync(x => x.Id == id);
            return spotPortfolio;
        }

        public async Task UpdateSpotPortfolio(SpotPortfolio spotPortfolio, CancellationToken cancellation)
        {
            _context.Set<SpotPortfolio>().Update(spotPortfolio);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task UpdateSpotPortfolios(List<SpotPortfolio> spotPortfolios, CancellationToken cancellation)
        {
            _context.Set<SpotPortfolio>().UpdateRange(spotPortfolios);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task<SpotPortfolio> GetSpotPortfolioByUserId(string userId)
        {
            var spotPortfolio = await _context.Set<SpotPortfolio>().FirstOrDefaultAsync(x => x.UserId == userId);
            return spotPortfolio;
        }

        public async Task AddSpotPortfolio(SpotPortfolio spotportfolio, CancellationToken cancellation)
        {
            await _context.Set<SpotPortfolio>().AddAsync(spotportfolio);
            await _context.SaveChangesAsync(cancellation);
        }
    }
}
