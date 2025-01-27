using TradingApp.Application.DataTransferObjects.Transaction;

namespace TradingApp.Application.Services.FuturesTransactionsService
{
    public interface IFuturesTransactionService
    {
        Task<RequestResult> CloseFuturesTransaction(int id, int portfolioId, CancellationToken cancellation);

        Task<RequestResult<IEnumerable<FuturesTransactionDto>>>
            GetFuturesTransactionsByPortfolioId(int portfolioId);

        Task<RequestResult> CalculateTransactionsProfits(CancellationToken cancellation);

        Task<RequestResult> EditFuturesTransaction(FuturesTransactionDto futuresTransactionDto,
            CancellationToken cancellation);
    }
}
