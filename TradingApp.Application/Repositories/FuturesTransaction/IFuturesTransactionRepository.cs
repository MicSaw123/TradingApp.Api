using TradingApp.Domain.Futures;

namespace TradingApp.Application.Repositories.TransactionRepository.FuturesTransactionRepository
{
    public interface IFuturesTransactionRepository
    {
        Task UpdateFuturesTransaction(FuturesTransaction transaction, CancellationToken cancellation);

        Task UpdateFuturesTransactionRange(List<FuturesTransaction> futuresTransactions, CancellationToken cancellation);

        Task<IEnumerable<FuturesTransaction>> GetActiveFuturesTransactionsByPortfolioId
            (int portfolioId);

        Task<FuturesTransaction> GetActiveFuturesTransactionById(int id);

        Task AddFuturesTransaction(FuturesTransaction futuresTransaction, CancellationToken cancellation);

    }
}
