using TradingApp.Domain.Futures;

namespace TradingApp.Application.Repositories.FuturesTransactionToOpenRepository
{
    public interface IFuturesTransactionToOpenRepository
    {
        Task AddFuturesTransactionToOpen(FuturesTransactionToOpen futuresTransactionToOpen,
            CancellationToken cancellation);

        Task RemoveFuturesTransactionToOpen(FuturesTransactionToOpen futuresTransactionToOpen, CancellationToken cancellation);

        Task<IEnumerable<FuturesTransactionToOpen>> GetFuturesTransactionsToOpenByPortfolioId(int portfolioId);

        Task<FuturesTransactionToOpen> GetFuturesTransactionToOpenById(int id);

        Task<IEnumerable<FuturesTransactionToOpen>> GetFuturesTransactionsToOpen();

        Task EditFuturesTransactionToOpen(FuturesTransactionToOpen futuresTransactionToOpen,
            CancellationToken cancellation);
    }
}
