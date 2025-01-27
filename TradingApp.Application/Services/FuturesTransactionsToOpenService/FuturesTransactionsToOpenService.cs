using AutoMapper;
using TradingApp.Application.DataTransferObjects.Futures;
using TradingApp.Application.Repositories.DbTransactionRepository;
using TradingApp.Application.Repositories.FuturesTransactionToOpenRepository;
using TradingApp.Application.Services.CoinService;
using TradingApp.Application.Services.FuturesPortfoliosService;
using TradingApp.Domain.Errors.TransactionToOpenErrors;
using TradingApp.Domain.Futures;

namespace TradingApp.Application.Services.FuturesTransactionsToOpenService
{
    public class FuturesTransactionsToOpenService : IFuturesTransactionsToOpenService
    {
        private readonly IFuturesTransactionToOpenRepository _futuresTransactionToOpenRepository;
        private readonly IDbTransactionRepository _dbTransaction;
        private readonly IMapper _mapper;
        private readonly IFuturesPortfolioService _futuresPortfolioService;
        private readonly ICoinService _coinService;

        public FuturesTransactionsToOpenService(IFuturesTransactionToOpenRepository futuresTransactionToOpenRepository,
            IDbTransactionRepository dbTransaction, IMapper mapper, IFuturesPortfolioService futuresPortfolioService,
            ICoinService coinService)
        {
            _futuresTransactionToOpenRepository = futuresTransactionToOpenRepository;
            _dbTransaction = dbTransaction;
            _mapper = mapper;
            _futuresPortfolioService = futuresPortfolioService;
            _coinService = coinService;
        }

        public async Task<RequestResult> AddFuturesTransactionToOpen(FuturesTransactionToOpenDto futuresTransactionToOpenDto,
            CancellationToken cancellation)
        {
            var futuresTransactionToOpen = _mapper.Map<FuturesTransactionToOpen>(futuresTransactionToOpenDto);
            using var dbTransaction = _dbTransaction.BeginTransaction();
            if (futuresTransactionToOpen != null)
            {
                try
                {
                    await _futuresTransactionToOpenRepository.AddFuturesTransactionToOpen(futuresTransactionToOpen, cancellation);
                    await _futuresPortfolioService.SubtractBalance(futuresTransactionToOpenDto.FuturesPortfolioId,
                        futuresTransactionToOpen.MoneyInput, cancellation);
                    dbTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    return RequestResult.Failure(TransactionToOpenError.ErrorAddTransactionToOpen);
                }
            }
            return RequestResult.Success();
        }

        public async Task<RequestResult> CancelFuturesTransactionToOpen(int id, int futuresPortfolioId,
            CancellationToken cancellation)
        {
            var futuresTransaction = await _futuresTransactionToOpenRepository.GetFuturesTransactionToOpenById(id);
            if (futuresTransaction is null)
            {
                return RequestResult.Failure(TransactionToOpenError.ErrorCancelTransactionToOpen);
            }
            await _futuresPortfolioService.
                AddBalance(futuresPortfolioId, futuresTransaction.MoneyInput, cancellation);
            await _futuresTransactionToOpenRepository.RemoveFuturesTransactionToOpen(futuresTransaction, cancellation);
            return RequestResult.Success();
        }

        public async Task<RequestResult> EditFuturesTransactionToOpen(FuturesTransactionToOpenDto futuresTransactionToOpenDto,
            CancellationToken cancellation)
        {
            using var dbTransaction = _dbTransaction.BeginTransaction();
            try
            {
                var transaction = await _futuresTransactionToOpenRepository
                    .GetFuturesTransactionToOpenById(futuresTransactionToOpenDto.Id);
                if (transaction.MoneyInput != futuresTransactionToOpenDto.MoneyInput)
                {
                    if (transaction.MoneyInput < futuresTransactionToOpenDto.MoneyInput)
                    {
                        await _futuresPortfolioService.
                        SubtractBalance(transaction.FuturesPortfolioId,
                            futuresTransactionToOpenDto.MoneyInput - transaction.MoneyInput, cancellation);
                    }
                    else
                    {
                        await _futuresPortfolioService
                        .AddBalance(transaction.FuturesPortfolioId,
                            transaction.MoneyInput - futuresTransactionToOpenDto.MoneyInput, cancellation);
                    }
                }
                transaction.BuyingPrice = futuresTransactionToOpenDto.BuyingPrice;
                transaction.TakeProfitPrice = futuresTransactionToOpenDto.TakeProfitPrice;
                transaction.ClosingPrice = futuresTransactionToOpenDto.ClosingPrice;
                transaction.MoneyInput = futuresTransactionToOpenDto.MoneyInput;
                transaction.Leverage = futuresTransactionToOpenDto.Leverage;
                await _futuresTransactionToOpenRepository.EditFuturesTransactionToOpen(transaction, cancellation);
                dbTransaction.Commit();
            }
            catch (Exception ex)
            {
                dbTransaction.Rollback();
                return RequestResult.Failure(TransactionToOpenError.ErrorEditTransactionToOpen);
            }
            return RequestResult.Success();
        }

        public async Task<RequestResult> OpenFuturesTransactionToOpen(CancellationToken cancellation)
        {
            var transactionsToOpen = await _futuresTransactionToOpenRepository.GetFuturesTransactionsToOpen();
            foreach (var transactionToOpen in transactionsToOpen)
            {
                var coin = await _coinService.GetCoinBySymbol(transactionToOpen.CoinSymbol);
                using var dbTransaction = _dbTransaction.BeginTransaction();
                try
                {
                    if (transactionToOpen.BuyingPrice >= coin.Result.Price)
                    {
                        var futuresTransaction = _mapper.Map<FuturesTransaction>(transactionToOpen);
                        futuresTransaction.IsActive = true;
                        futuresTransaction.AmountOfCoin = futuresTransaction.MoneyInput / coin.Result.Price;
                        futuresTransaction.BuyingPrice = coin.Result.Price;
                        await _futuresTransactionToOpenRepository.AddFuturesTransactionToOpen(transactionToOpen, cancellation);
                        await _futuresTransactionToOpenRepository.RemoveFuturesTransactionToOpen(transactionToOpen, cancellation);
                        dbTransaction.Commit();
                        return RequestResult.Success();
                    }
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    throw;
                }
            }
            return RequestResult.Failure(TransactionToOpenError.ErrorOpenAwaitingTransactionToOpen);
        }
    }
}
