namespace TradingApp.Application.Services.FuturesPortfoliosService
{
    public interface IFuturesPortfolioService
    {
        Task<RequestResult> AddBalance(int portfolioId, float balanceToAdd, CancellationToken cancellation);

        Task<RequestResult> SubtractBalance(int portfolioId, float balanceToSubtract, CancellationToken cancellation);
    }
}
