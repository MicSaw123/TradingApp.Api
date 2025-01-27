using TradingApp.Domain.Spot;

namespace TradingApp.Application.Repositories.SpotTransactionToAdd
{
    public interface ISpotTransactionToOpenRepository
    {
        Task AddSpotTransactionToOpen(SpotTransactionToOpen futuresTransactionToOpen, CancellationToken cancellation);

        Task RemoveSpotTransactionToOpen(SpotTransactionToOpen futuresTransactionToOpen, CancellationToken cancellation);

        Task<IEnumerable<SpotTransactionToOpen>> GetSpotTransactionsToOpenByPortfolioId(int portfolioId);

        Task<SpotTransactionToOpen> GetSpotTransactionToOpenById(int id);

        Task<IEnumerable<SpotTransactionToOpen>> GetSpotTransactionsToOpen();

        Task EditSpotTransactionToOpen(SpotTransactionToOpen futuresTransactionToOpen,
            CancellationToken cancellation);

    }
}
