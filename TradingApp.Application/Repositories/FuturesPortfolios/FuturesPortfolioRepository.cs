using Microsoft.EntityFrameworkCore;
using TradingApp.Application.Services.Interfaces.Database;
using TradingApp.Domain.Errors.Errors.SpotPortfolioErrors;
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

        public async Task<RequestResult> AddBalance(int portfolioId, float balanceToAdd, CancellationToken cancellation)
        {
            var portfolio = await _context.Set<FuturesPortfolio>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == portfolioId);
            if (portfolio is not null)
            {
                portfolio.Balance += balanceToAdd;
                _context.Set<FuturesPortfolio>().Update(portfolio);
                await _context.SaveChangesAsync(cancellation);
            }
            else
            {
                return RequestResult.Failure(PortfolioError.ErrorGetPortfolioById);
            }
            return RequestResult.Success();
        }

        public async Task<RequestResult> SubtractBalance(int portfolioId, float balanceToSubtract,
            CancellationToken cancellation)
        {
            var portfolio = await _context.Set<FuturesPortfolio>().AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == portfolioId);
            if (portfolio is not null)
            {
                portfolio.Balance -= balanceToSubtract;
                if (portfolio.Balance < 0)
                {
                    return RequestResult.Failure(PortfolioError.NonSufficientFunds);
                }
                _context.Set<FuturesPortfolio>().Update(portfolio);
                await _context.SaveChangesAsync(cancellation);
            }
            else
            {
                return RequestResult.Failure(PortfolioError.ErrorGetPortfolioById);
            }
            return RequestResult.Success();
        }
    }
}
