using Microsoft.EntityFrameworkCore;
using TradingApp.Application.Services.Interfaces.Database;
using TradingApp.Domain.Futures;

namespace TradingApp.Application.Repositories.FuturesPortfolios
{
    public class FuturesPortfolioRepository : IFuturesPortfolioRepository
    {
        private readonly IDbContext _context;

        public FuturesPortfolioRepository(IDbContext context)
        {
            _context = context;
        }

        public async Task AddFuturesPortfolio(FuturesPortfolio futuresPortfolio, CancellationToken cancellation)
        {
            await _context.Set<FuturesPortfolio>().AddAsync(futuresPortfolio);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task<FuturesPortfolio> GetFuturesPortfolioById(int id)
        {
            var portfolio = await _context.Set<FuturesPortfolio>().FirstOrDefaultAsync(x => x.Id == id);
            return portfolio;
        }

        public async Task<List<FuturesPortfolio>> GetFuturesPortfolios()
        {
            var futuresPortfolios = _context.Set<FuturesPortfolio>().ToList();
            return futuresPortfolios;
        }

        public async Task UpdateFuturesPortfolio(FuturesPortfolio portfolio, CancellationToken cancellation)
        {
            _context.Set<FuturesPortfolio>().Update(portfolio);
            await _context.SaveChangesAsync(cancellation); ;
        }

        public async Task UpdateFuturesPortfolios(List<FuturesPortfolio> futuresTransactions, CancellationToken cancellation)
        {
            _context.Set<FuturesPortfolio>().UpdateRange(futuresTransactions);
            await _context.SaveChangesAsync(cancellation);
        }
    }
}
