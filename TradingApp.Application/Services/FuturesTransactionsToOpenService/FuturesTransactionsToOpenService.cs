using TradingApp.Application.DataTransferObjects.Futures;
using TradingApp.Application.Repositories.FuturesTransactionToOpenRepository;

namespace TradingApp.Application.Services.FuturesTransactionsToOpenService
{
    public class FuturesTransactionsToOpenService : IFuturesTransactionsToOpenService
    {
        private readonly IFuturesTransactionToOpenRepository _futuresTransactionToOpenRepository;

        public FuturesTransactionsToOpenService(IFuturesTransactionToOpenRepository futuresTransactionToOpenRepository)
        {
            _futuresTransactionToOpenRepository = futuresTransactionToOpenRepository;
        }

        public async Task<RequestResult> AddFuturesTransactionToOpen(FuturesTransactionToOpenDto futuresTransactionToOpenDto,
            CancellationToken cancellation)
        {
            return await _futuresTransactionToOpenRepository
                .AddFuturesTransactionToOpen(futuresTransactionToOpenDto, cancellation);
        }

        public async Task<RequestResult> CancelFuturesTransactionToOpen(int id, int futuresPortfolioId,
            CancellationToken cancellation)
        {
            return await _futuresTransactionToOpenRepository
                .CancelFuturesTransactionToOpen(id, futuresPortfolioId, cancellation);
        }

        public async Task<RequestResult> EditFuturesTransactionToOpen(FuturesTransactionToOpenDto futuresTransactionToOpenDto,
            CancellationToken cancellation)
        {
            return await _futuresTransactionToOpenRepository
                .EditFuturesTransactionToOpen(futuresTransactionToOpenDto, cancellation);
        }

        public async Task<RequestResult> OpenFuturesTransactionToOpen(CancellationToken cancellation)
        {
            return await _futuresTransactionToOpenRepository.OpenFuturesTransactionToOpen(cancellation);
        }
    }
}
