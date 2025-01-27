using AutoMapper;
using TradingApp.Application.DataTransferObjects.Transaction;
using TradingApp.Application.Repositories.DbTransactionRepository;
using TradingApp.Application.Repositories.FuturesPortfolios;
using TradingApp.Application.Repositories.TransactionRepository.FuturesTransactionRepository;
using TradingApp.Application.Services.CoinService;
using TradingApp.Domain.Errors.Errors.TransactionErrors;
using TradingApp.Domain.Futures;

namespace TradingApp.Application.Services.FuturesTransactionsService
{
    public class FuturesTransactionService : IFuturesTransactionService
    {
        private readonly IFuturesTransactionRepository _futuresTransactionRepository;
        private readonly IFuturesPortfolioRepository _futuresPortfolioRepository;
        private readonly IMapper _mapper;
        private readonly ICoinService _coinService;
        private readonly IDbTransactionRepository _dbTransaction;

        public FuturesTransactionService(IFuturesTransactionRepository futuresTransactionRepository,
            IFuturesPortfolioRepository futuresPortfolioRepository, IMapper mapper, ICoinService coinService,
            IDbTransactionRepository dbTransaction)
        {
            _futuresTransactionRepository = futuresTransactionRepository;
            _futuresPortfolioRepository = futuresPortfolioRepository;
            _mapper = mapper;
            _coinService = coinService;
            _dbTransaction = dbTransaction;
        }

        public async Task<RequestResult> CalculateTransactionsProfits(CancellationToken cancellation)
        {
            var futuresPortfolios = await _futuresPortfolioRepository.GetFuturesPortfolios();
            foreach (var futuresPortfolio in futuresPortfolios)
            {
                float totalPortfolioProfit = 0;
                var futuresTransactions = await _futuresTransactionRepository
                    .GetActiveFuturesTransactionsByPortfolioId(futuresPortfolio.Id);
                foreach (var futuresTransaction in futuresTransactions)
                {
                    var coin = await _coinService.GetCoinBySymbol(futuresTransaction.CoinSymbol);
                    futuresTransaction.TransactionProfit += (futuresTransaction.AmountOfCoin * coin.Result.Price)
                        - (futuresTransaction.AmountOfCoin * futuresTransaction.BuyingPrice);
                    totalPortfolioProfit += futuresTransaction.TransactionProfit;
                }
                futuresPortfolio.DailyProfit += totalPortfolioProfit;
                futuresPortfolio.WeeklyProfit += totalPortfolioProfit;
                futuresPortfolio.MonthlyProfit += totalPortfolioProfit;
                await _futuresTransactionRepository.UpdateFuturesTransactionRange(futuresTransactions.ToList(), cancellation);
            }
            await _futuresPortfolioRepository.UpdateFuturesPortfolios(futuresPortfolios, cancellation);
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
                    var portfolio = await _futuresPortfolioRepository.GetFuturesPortfolioById(transactionToClose.FuturesPortfolioId);
                    portfolio.Balance += transactionToClose.TransactionProfit + transactionToClose.MoneyInput;
                    await _futuresPortfolioRepository.UpdateFuturesPortfolio(portfolio, cancellation);
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
            await _futuresTransactionRepository.UpdateFuturesTransaction(futuresTransaction, cancellation);
            if (cancellation.IsCancellationRequested)
            {
                return RequestResult.Failure(TransactionError.ErrorEditTransaction);
            }
            return RequestResult.Success();
        }

        public async Task<RequestResult<IEnumerable<FuturesTransactionDto>>>
            GetFuturesTransactionByPortfolioId(int portfolioId)
        {
            var result = await _futuresTransactionRepository
                .GetActiveFuturesTransactionsByPortfolioId(portfolioId);
            if (result is null)
            {
                return RequestResult<IEnumerable<FuturesTransactionDto>>.Failure(TransactionError.ErrorGetTransactionsByPortfolioId);
            }
            var futuresTransactionDtos = _mapper.Map<IEnumerable<FuturesTransactionDto>>(result);
            return RequestResult<IEnumerable<FuturesTransactionDto>>.Success(futuresTransactionDtos);
        }

        public async Task<RequestResult<IEnumerable<FuturesTransactionDto>>> GetFuturesTransactionsByPortfolioId(int portfolioId)
        {
            var result = await _futuresTransactionRepository.GetActiveFuturesTransactionsByPortfolioId(portfolioId);
            var futuresTransactionsDto = _mapper.Map<IEnumerable<FuturesTransactionDto>>(result);
            return RequestResult<IEnumerable<FuturesTransactionDto>>.Success(futuresTransactionsDto);
        }
    }
}
