using TradingApp.Application.DataTransferObjects.Transaction;
using TradingApp.Domain.Futures;

namespace TradingApp.Application.Services.FuturesTransactionsService
{
    public interface IFuturesTransactionService
    {
        Task<RequestResult> CloseFuturesTransaction(int id, int portfolioId, CancellationToken cancellation);

        Task<RequestResult<IEnumerable<FuturesTransactionDto>>>
            GetActiveFuturesTransactionsByPortfolioId(int portfolioId);

        Task<RequestResult> CalculateTransactionsProfits(CancellationToken cancellation);

        Task<RequestResult> EditFuturesTransaction(FuturesTransactionDto futuresTransactionDto,
            CancellationToken cancellation);

        Task<RequestResult<IEnumerable<FuturesTransactionDto>>>
            GetInactiveFuturesTransactionsByPortfolioId(int portfolioId);

        Task<FuturesTransaction> GetFuturesTransactionByCoinSymbol(int portfolioId, string coinSymbol);

        Task AddFuturesTransaction(FuturesTransaction futuresTransaction,
            CancellationToken cancellation);
    }
}
