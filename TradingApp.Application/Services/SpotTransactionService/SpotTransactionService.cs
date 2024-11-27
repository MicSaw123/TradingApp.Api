using TradingApp.Application.DataTransferObjects.Transaction;
using TradingApp.Application.Repositories.SpotTransactionRepository;

namespace TradingApp.Application.Services.SpotTransactionService
{
    public class SpotTransactionService : ISpotTransactionService
    {
        private readonly ISpotTransactionRepository _spotTransactionRepository;

        public SpotTransactionService(ISpotTransactionRepository spotTransactionRepository)
        {
            _spotTransactionRepository = spotTransactionRepository;
        }

        public async Task<RequestResult> CalculateSpotTransactionProfit
            (CancellationToken cancellation = default)
        {
            var result = await _spotTransactionRepository
                .CalculateSpotTransactionProfit(cancellation);
            return result;
        }

        public async Task<RequestResult> CloseExistingSpotTransactions(CancellationToken cancellation)
        {
            var result = await _spotTransactionRepository.CloseExistingTransaction(cancellation);
            return result;
        }

        public async Task<RequestResult> EditSpotTransaction(SpotTransactionDto spotTransactionDto, CancellationToken cancellation)
        {
            var result = await _spotTransactionRepository.EditSpotTransaction(spotTransactionDto, cancellation);
            return result;
        }
    }
}
