using TradingApp.Domain.Spot;

namespace TradingApp.Application.Repositories.SpotTransactionRepository
{
    public interface ISpotTransactionRepository
    {
        Task<IEnumerable<SpotTransaction>> GetActiveSpotTransactionsByPortfolioId(int portfolioId);

        Task<IEnumerable<SpotTransaction>> GetInactiveSpotTransactionsByPortfolioId(int potfolioId);

        Task<SpotTransaction> GetSpotTransactionById(int transactionId);

        Task UpdateSpotTransaction(SpotTransaction spotTransaction, CancellationToken cancellation);

        Task UpdateSpotTransactionRange(List<SpotTransaction> spotTransactions, CancellationToken cancellation);

        Task AddSpotTransaction(SpotTransaction spotTransaction, CancellationToken cancellation);

        Task<IEnumerable<SpotTransaction>> GetSpotTransactionsWithSellingPrice();

        Task<SpotTransaction> GetExistingSpotTransactionWithSpecifiedCoinSymbol(int portfolioId, string coinSymbol);
    }
}
