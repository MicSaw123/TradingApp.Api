using TradingApp.Application.DataTransferObjects.Spot;

namespace TradingApp.Application.Repositories.SpotTransactionToAdd
{
    public interface ISpotTransactionToOpenRepository
    {
        Task<RequestResult> OpenWaitingSpotTransaction(CancellationToken cancellation);

        Task<RequestResult> AddAwaitingTransactionToSpotPortfolio(
            SpotTransactionToOpenDto transaction,
            CancellationToken cancellation = default);

        Task<RequestResult> CancelAwaitingSpotTransaction(int id, int spotPortfolioId, CancellationToken cancellation);

        Task<RequestResult> EditAwaitingSpotTransaction(SpotTransactionToOpenDto spotTransactionToOpenDto,
            CancellationToken cancellation);
    }
}
