using TradingApp.Application.DataTransferObjects.Futures;

namespace TradingApp.Application.Repositories.FuturesTransactionToOpenRepository
{
    public interface IFuturesTransactionToOpenRepository
    {
        Task<RequestResult> AddFuturesTransactionToOpen(FuturesTransactionToOpenDto futuresTransactionToOpenDto,
            CancellationToken cancellation);

        Task<RequestResult> OpenFuturesTransactionToOpen(CancellationToken cancellation);

        Task<RequestResult> CancelFuturesTransactionToOpen(int id, int futuresPortfolioId, CancellationToken cancellation);

        Task<RequestResult> EditFuturesTransactionToOpen(FuturesTransactionToOpenDto futuresTransactionToOpenDto,
            CancellationToken cancellation);
    }
}
