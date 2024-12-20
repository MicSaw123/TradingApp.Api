
using TradingApp.Application.DataTransferObjects.Spot;
using TradingApp.Application.Repositories.SpotTransactionToAdd;

namespace TradingApp.Application.Services.SpotTransactionToOpenService
{
    public class SpotTransactionToOpenService : ISpotTransactionToOpenService
    {
        private readonly ISpotTransactionToOpenRepository _spotTransactionToOpenRepository;

        public SpotTransactionToOpenService(ISpotTransactionToOpenRepository
            spotTransactionToOpenRepository)
        {
            _spotTransactionToOpenRepository = spotTransactionToOpenRepository;
        }

        public async Task<RequestResult> AddAwaitingTransactionToSpotPortfolio
            (SpotTransactionToOpenDto spotTransactionToOpen,
            CancellationToken cancellation = default)
        {
            var result = await _spotTransactionToOpenRepository.
                AddAwaitingTransactionToSpotPortfolio(spotTransactionToOpen, cancellation);
            return result;
        }

        public async Task<RequestResult> CancelAwaitingSpotTransaction(int id, int spotPortfolioId,
            CancellationToken cancellation)
        {
            var result = await _spotTransactionToOpenRepository.CancelAwaitingSpotTransaction
                (id, spotPortfolioId, cancellation);
            return result;
        }

        public async Task<RequestResult> EditAwaitingSpotTransaction(SpotTransactionToOpenDto spotTransactionToOpenDto,
            CancellationToken cancellation)
        {
            var result = await _spotTransactionToOpenRepository.EditAwaitingSpotTransaction
                (spotTransactionToOpenDto, cancellation);
            return result;
        }

        public async Task<RequestResult> OpenWaitingSpotTransaction(CancellationToken cancellation = default)
        {
            var result = await _spotTransactionToOpenRepository.
                OpenWaitingSpotTransaction(cancellation);
            return result;
        }
    }
}
