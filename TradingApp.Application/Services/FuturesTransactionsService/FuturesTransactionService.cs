using TradingApp.Application.DataTransferObjects.Transaction;
using TradingApp.Application.Repositories.TransactionRepository.FuturesTransactionRepository;

namespace TradingApp.Application.Services.FuturesTransactionsService
{
    public class FuturesTransactionService : IFuturesTransactionService
    {
        private readonly IFuturesTransactionRepository _futuresTransactionRepository;

        public FuturesTransactionService(IFuturesTransactionRepository futuresTransactionRepository)
        {
            _futuresTransactionRepository = futuresTransactionRepository;
        }

        public async Task<RequestResult> CalculateTransactionsProfits(CancellationToken cancellation)
        {
            var result = await _futuresTransactionRepository.CalculateTransactionsProfits(cancellation);
            return result;
        }

        public async Task<RequestResult> CloseFuturesTransaction(int id, int portfolioId, CancellationToken cancellation)
        {
            var result = await _futuresTransactionRepository.CloseFuturesTransaction(id, portfolioId, cancellation);
            return result;
        }

        public async Task<RequestResult> EditFuturesTransaction(FuturesTransactionDto futuresTransactionDto,
            CancellationToken cancellation)
        {
            var result = await _futuresTransactionRepository.EditFuturesTransaction(futuresTransactionDto, cancellation);
            return result;
        }

        public async Task<RequestResult<IEnumerable<FuturesTransactionDto>>>
            GetFuturesTransactionByPortfolioId(int portfolioId, CancellationToken cancellation)
        {
            var result = await _futuresTransactionRepository
                .GetFuturesTransactionByPortfolioId(portfolioId, cancellation);
            return result;
        }
    }
}
