namespace TradingApp.Application.Repositories.FuturesPortfolios
{
    public interface IFuturesPortfolioRepository
    {
        Task<RequestResult> AddBalance(int portfolioId, float balanceToAdd, CancellationToken cancellation);

        Task<RequestResult> SubtractBalance(int portfolioId, float balanceToSubtract, CancellationToken cancellation);
    }
}
