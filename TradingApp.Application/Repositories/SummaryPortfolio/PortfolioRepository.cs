using Microsoft.EntityFrameworkCore;
using TradingApp.Application.Services.Interfaces.Database;
using TradingApp.Domain.SummaryPortfolio;

namespace TradingApp.Application.Repositories.SummaryPortfolio
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly IDbContext _context;

        public PortfolioRepository(IDbContext context)
        {
            _context = context;
        }

        public async Task AddPortfolio(Portfolio portfolio, CancellationToken cancellation)
        {
            await _context.Set<Portfolio>().AddAsync(portfolio);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task<Portfolio> GetPortfolioByUserId(string userId)
        {
            var portfolio = await _context.Set<Portfolio>().FirstOrDefaultAsync(x => x.UserId == userId);
            return portfolio;
        }
    }
}
