using AutoMapper;
using TradingApp.Application.DataTransferObjects.Transaction;
using TradingApp.Application.Repositories.DbTransactionRepository;
using TradingApp.Application.Repositories.TransactionRepository.FuturesTransactionRepository;
using TradingApp.Application.Services.CoinService;
using TradingApp.Application.Services.FuturesPortfoliosService;
using TradingApp.Domain.Errors.Errors.TransactionErrors;
using TradingApp.Domain.Futures;

namespace TradingApp.Application.Services.FuturesTransactionsService
{
    public class FuturesTransactionService : IFuturesTransactionService
    {
        private readonly IFuturesTransactionRepository _futuresTransactionRepository;
        private readonly IFuturesPortfolioService _futuresPortfolioService;
        private readonly IMapper _mapper;
        private readonly ICoinService _coinService;
        private readonly IDbTransactionRepository _dbTransaction;

        public FuturesTransactionService(IFuturesTransactionRepository futuresTransactionRepository,
            IFuturesPortfolioService futuresPortfolioService, IMapper mapper, ICoinService coinService,
            IDbTransactionRepository dbTransaction)
        {
            _futuresTransactionRepository = futuresTransactionRepository;
            _futuresPortfolioService = futuresPortfolioService;
            _mapper = mapper;
            _coinService = coinService;
            _dbTransaction = dbTransaction;
        }

        public async Task<RequestResult> CalculateTransactionsProfits(CancellationToken cancellation)
        {
            var futuresPortfolios = await _futuresPortfolioService.GetFuturesPortfolios();
            foreach (var futuresPortfolio in futuresPortfolios)
            {
                float totalPortfolioProfit = 0;
                var futuresTransactions = await _futuresTransactionRepository
                    .GetActiveFuturesTransactionsByPortfolioId(futuresPortfolio.Id);
                foreach (var futuresTransaction in futuresTransactions)
                {
                    var coin = await _coinService.GetCoinBySymbol(futuresTransaction.CoinSymbol);
                    futuresTransaction.TransactionProfit += (futuresTransaction.AmountOfCoin * coin.Result.Price)
                        - futuresTransaction.MoneyInput;
                    totalPortfolioProfit += futuresTransaction.TransactionProfit;
                }
                futuresPortfolio.DailyProfit += totalPortfolioProfit;
                futuresPortfolio.WeeklyProfit += totalPortfolioProfit;
                futuresPortfolio.MonthlyProfit += totalPortfolioProfit;
                futuresPortfolio.AllocatedBalance += totalPortfolioProfit;
                await _futuresTransactionRepository.UpdateFuturesTransactionRange(futuresTransactions.ToList(), cancellation);
            }
            await _futuresPortfolioService.UpdateFuturesPortfolios(futuresPortfolios, cancellation);
            return RequestResult.Success();
        }

        public async Task<RequestResult> CloseFuturesTransaction(int id, int portfolioId, CancellationToken cancellation)
        {
            var transactionToClose = await _futuresTransactionRepository
                .GetActiveFuturesTransactionById(portfolioId);
            if (transactionToClose != null)
            {
                using var dbTransaction = _dbTransaction.BeginTransaction();
                try
                {
                    var coin = await _coinService.GetCoinBySymbol(transactionToClose.CoinSymbol);
                    transactionToClose.TransactionProfit =
                        (transactionToClose.AmountOfCoin * coin.Result.Price) - transactionToClose.MoneyInput;
                    var portfolio = await _futuresPortfolioService
                        .GetFuturesPortfolioById(transactionToClose.FuturesPortfolioId);
                    portfolio.DisposableBalance += transactionToClose.TransactionProfit + transactionToClose.MoneyInput;
                    await _futuresPortfolioService.UpdateFuturesPortfolio(portfolio, cancellation);
                    transactionToClose.IsActive = false;
                    await _futuresTransactionRepository.UpdateFuturesTransaction(transactionToClose, cancellation);
                    dbTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    return RequestResult.Failure(TransactionError.ErrorCloseTransaction);
                }
            }
            return RequestResult.Success();
        }

        public async Task<RequestResult> EditFuturesTransaction(FuturesTransactionDto futuresTransactionDto,
            CancellationToken cancellation)
        {
            var futuresTransaction = _mapper.Map<FuturesTransaction>(futuresTransactionDto);
            futuresTransaction.LastEditTransactionDate = DateOnly.FromDateTime(DateTime.Now);
            await _futuresTransactionRepository.UpdateFuturesTransaction(futuresTransaction, cancellation);
            if (cancellation.IsCancellationRequested)
            {
                return RequestResult.Failure(TransactionError.ErrorEditTransaction);
            }
            return RequestResult.Success();
        }

        public async Task<RequestResult<IEnumerable<FuturesTransactionDto>>>
            GetInactiveFuturesTransactionsByPortfolioId(int portfolioId)
        {
            var inactiveFuturesTransactions = await
                _futuresTransactionRepository.GetInactiveFuturesTransactionsByPortfolioId(portfolioId);
            if (inactiveFuturesTransactions is null)
            {
                return RequestResult<IEnumerable<FuturesTransactionDto>>
                    .Failure(TransactionError.ErrorGetTransactionsByPortfolioId);
            }

            var inactiveFuturesTransactionsDto = _mapper
                .Map<IEnumerable<FuturesTransactionDto>>(inactiveFuturesTransactions);
            return RequestResult<IEnumerable<FuturesTransactionDto>>.Success(inactiveFuturesTransactionsDto);
        }

        public async Task<FuturesTransaction> GetFuturesTransactionByCoinSymbol(int portfolioId, string coinSymbol)
        {
            var futuresTransaction = await
                _futuresTransactionRepository.GetFuturesTransactionByCoinSymbol(portfolioId, coinSymbol);
            return futuresTransaction;
        }

        public async Task
            AddFuturesTransaction(FuturesTransaction futuresTransaction, CancellationToken cancellation)
        {
            await _futuresTransactionRepository.AddFuturesTransaction(futuresTransaction, cancellation);
        }


        public async Task<RequestResult<IEnumerable<FuturesTransactionDto>>>
            GetActiveFuturesTransactionsByPortfolioId(int portfolioId)
        {
            var result = await
                _futuresTransactionRepository.GetActiveFuturesTransactionsByPortfolioId(portfolioId);
            var futuresTransactionsDto = _mapper.Map<IEnumerable<FuturesTransactionDto>>(result);
            return RequestResult<IEnumerable<FuturesTransactionDto>>.Success(futuresTransactionsDto);
        }

    }
}
