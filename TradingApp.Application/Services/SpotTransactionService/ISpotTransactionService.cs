using TradingApp.Application.DataTransferObjects.Transaction;
using TradingApp.Domain.Spot;

namespace TradingApp.Application.Services.SpotTransactionService
{
    public interface ISpotTransactionService
    {
        Task<RequestResult> CloseExistingSpotTransactions(CancellationToken cancellation);

        Task<RequestResult> CloseExistingTransactionsManually(SpotTransactionDto spotTransactionDto,
            CancellationToken cancellation);

        Task<RequestResult> EditSpotTransaction(SpotTransactionDto spotTransactionDto, CancellationToken cancellation);

        Task<RequestResult> CalculateSpotTransactionProfit(CancellationToken cancellation);

        Task<RequestResult<IEnumerable<SpotTransaction>>> GetActiveTransactionsByPortfolioId(int portfolioId);

        Task<SpotTransaction> GetExistingSpotTransactionWithSpecifiedCoinSymbol(int portfolioId, string coinSymbol);

        Task AddSpotTransaction(SpotTransaction spotTransaction, CancellationToken cancellation);

    }
}
