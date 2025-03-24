using TradingApp.Application.DataTransferObjects.Futures;
using TradingApp.Domain.Futures;

namespace TradingApp.Application.Services.FuturesPortfoliosService
{
    public interface IFuturesPortfolioService
    {
        Task<RequestResult> AddBalance(int portfolioId, float balanceToAdd, CancellationToken cancellation);

        Task<RequestResult> SubtractBalance(int portfolioId, float balanceToSubtract, CancellationToken cancellation);

        Task<RequestResult> AddFuturesPortfolio(FuturesPortfolio portfolio, CancellationToken cancellation);

        Task<RequestResult<FuturesPortfolioDto>> GetFuturesPortfolioDtoById(int portfolioId);

        Task<FuturesPortfolio> GetFuturesPortfolioById(int portfolioId);

        Task<List<FuturesPortfolio>> GetFuturesPortfolios();

        Task UpdateFuturesPortfolios(List<FuturesPortfolio> futuresPortfolios, CancellationToken cancellation);

        Task UpdateFuturesPortfolio(FuturesPortfolio futuresPortfolio, CancellationToken cancellation);
    }
}
