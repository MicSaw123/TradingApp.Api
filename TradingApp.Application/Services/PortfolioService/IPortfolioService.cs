using TradingApp.Application.DataTransferObjects.Portfolio;
using TradingApp.Domain.SummaryPortfolio;

namespace TradingApp.Application.Services.PortfolioService
{
    public interface IPortfolioService
    {
        Task<RequestResult<PortfolioDto>> GetPortfolioByUserId(string userId);

        Task<RequestResult> AddPortfolio(Portfolio portfolio, CancellationToken cancellation);
    }
}
