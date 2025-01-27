using TradingApp.Domain.Futures;

namespace TradingApp.Application.Repositories.FuturesPortfolios
{
    public interface IFuturesPortfolioRepository
    {
        Task<List<FuturesPortfolio>> GetFuturesPortfolios();

        Task<FuturesPortfolio> GetFuturesPortfolioById(int id);

        Task UpdateFuturesPortfolio(FuturesPortfolio portfolio, CancellationToken cancellation);

        Task UpdateFuturesPortfolios(List<FuturesPortfolio> futuresTransactions,
            CancellationToken cancellation);

        Task AddFuturesPortfolio(FuturesPortfolio futuresPortfolio, CancellationToken cancellation);
    }
}
