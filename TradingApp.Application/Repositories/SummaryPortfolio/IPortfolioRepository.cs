using TradingApp.Domain.SummaryPortfolio;

namespace TradingApp.Application.Repositories.SummaryPortfolio
{
    public interface IPortfolioRepository
    {
        Task AddPortfolio(Portfolio portfolio, CancellationToken cancellation);

        Task<Portfolio> GetPortfolioByUserId(string userId);
    }
}
