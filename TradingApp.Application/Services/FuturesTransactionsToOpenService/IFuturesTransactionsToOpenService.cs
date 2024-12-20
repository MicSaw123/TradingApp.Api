using TradingApp.Application.DataTransferObjects.Futures;

namespace TradingApp.Application.Services.FuturesTransactionsToOpenService
{
    public interface IFuturesTransactionsToOpenService
    {
        Task<RequestResult> AddFuturesTransactionToOpen(FuturesTransactionToOpenDto futuresTransactionToOpenDto,
    CancellationToken cancellation);

        Task<RequestResult> OpenFuturesTransactionToOpen(CancellationToken cancellation);

        Task<RequestResult> CancelFuturesTransactionToOpen(int id, int futuresPortfolioId, CancellationToken cancellation);

        Task<RequestResult> EditFuturesTransactionToOpen(FuturesTransactionToOpenDto futuresTransactionToOpenDto,
            CancellationToken cancellation);
    }
}
