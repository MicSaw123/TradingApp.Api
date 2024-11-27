using TradingApp.Application.DataTransferObjects.Transaction;

namespace TradingApp.Application.Repositories.SpotTransactionRepository
{
    public interface ISpotTransactionRepository
    {
        Task<RequestResult> CloseExistingTransaction(CancellationToken cancellation);

        Task<RequestResult> CalculateSpotTransactionProfit(CancellationToken cancellation);

        Task<RequestResult> CloseSpotTransactionManually(SpotTransactionDto spotTransactionDto,
            CancellationToken cancellation);

        Task<RequestResult> EditSpotTransaction(SpotTransactionDto spotTransactionDto, CancellationToken cancellation);
    }
}
