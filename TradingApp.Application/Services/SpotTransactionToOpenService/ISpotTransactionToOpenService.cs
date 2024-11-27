using TradingApp.Application.DataTransferObjects.Spot;

namespace TradingApp.Application.Services.SpotTransactionToOpenService
{
    public interface ISpotTransactionToOpenService
    {
        Task<RequestResult> OpenWaitingSpotTransaction(CancellationToken cancellation);

        Task<RequestResult> AddAwaitingTransactionToSpotPortfolio
    (SpotTransactionToOpenDto spotTransactionToOpen, CancellationToken cancellation = default);


        Task<RequestResult> CancelAwaitingSpotTransaction(int id, int spotPortfolioId, CancellationToken cancellation);

        Task<RequestResult> EditAwaitingSpotTransaction(SpotTransactionToOpenDto spotTransactionToOpenDto,
            CancellationToken cancellation);
    }
}
