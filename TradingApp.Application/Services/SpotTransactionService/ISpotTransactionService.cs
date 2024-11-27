using TradingApp.Application.DataTransferObjects.Transaction;

namespace TradingApp.Application.Services.SpotTransactionService
{
    public interface ISpotTransactionService
    {
        Task<RequestResult> CloseExistingSpotTransactions(CancellationToken cancellation);

        Task<RequestResult> CalculateSpotTransactionProfit(CancellationToken cancellation);

        Task<RequestResult> EditSpotTransaction(SpotTransactionDto spotTransactionDto, CancellationToken cancellation);
    }
}
