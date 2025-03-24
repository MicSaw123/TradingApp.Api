using AutoMapper;
using TradingApp.Application.DataTransferObjects.Futures;
using TradingApp.Application.DataTransferObjects.Transaction;
using TradingApp.Application.Repositories.DbTransactionRepository;
using TradingApp.Application.Repositories.FuturesTransactionToOpenRepository;
using TradingApp.Application.Services.CoinService;
using TradingApp.Application.Services.FuturesPortfoliosService;
using TradingApp.Application.Services.FuturesTransactionsService;
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
        private readonly IFuturesTransactionService _futuresTransactionService;

        public FuturesTransactionsToOpenService(IFuturesTransactionToOpenRepository futuresTransactionToOpenRepository,
            IDbTransactionRepository dbTransaction, IMapper mapper,
            IFuturesPortfolioService futuresPortfolioService,
            ICoinService coinService, IFuturesTransactionService futuresTransactionService)
        {
            _futuresTransactionToOpenRepository = futuresTransactionToOpenRepository;
            _dbTransaction = dbTransaction;
            _mapper = mapper;
            _futuresPortfolioService = futuresPortfolioService;
            _coinService = coinService;
            _futuresTransactionService = futuresTransactionService;
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

        public async Task<RequestResult<IEnumerable<FuturesTransactionToOpenDto>>>
            GetFuturesTransactionsToOpenById(int portfolioId)
        {
            var futuresTransactionsToOpen = await
                _futuresTransactionToOpenRepository.GetFuturesTransactionsToOpenByPortfolioId(portfolioId);
            if (futuresTransactionsToOpen is null)
            {
                return RequestResult<IEnumerable<FuturesTransactionToOpenDto>>
                    .Failure(TransactionToOpenError.ErrorGetAwaitingTransactionsToOpenById);
            }

            var futuresTransactionsToOpenDtos = _mapper
                .Map<IEnumerable<FuturesTransactionToOpenDto>>(futuresTransactionsToOpen);
            return RequestResult<IEnumerable<FuturesTransactionToOpenDto>>
                .Success(futuresTransactionsToOpenDtos);
        }

        public async Task<RequestResult> OpenFuturesTransactionToOpen(CancellationToken cancellation)
        {
            var futuresTransactionsToOpen = await
                      _futuresTransactionToOpenRepository.GetFuturesTransactionsToOpen();
            foreach (var futuresTransactionToOpen in futuresTransactionsToOpen)
            {
                var coin = await _coinService
                    .GetCoinBySymbol(futuresTransactionToOpen.CoinSymbol);
                using var dbTransaction = _dbTransaction.BeginTransaction();
                try
                {
                    if (futuresTransactionToOpen.BuyingPrice >= coin.Result.Price)
                    {
                        var futuresTransactionToAdd = _mapper.Map<FuturesTransaction>(futuresTransactionToOpen);
                        futuresTransactionToAdd.AmountOfCoin = futuresTransactionToAdd.MoneyInput
                                                               / coin.Result.Price;
                        futuresTransactionToAdd.IsActive = true;
                        futuresTransactionToAdd.OpenTransactionDate = DateOnly.FromDateTime(DateTime.Today);
                        futuresTransactionToAdd.BuyingPrice = coin.Result.Price;
                        futuresTransactionToAdd.CurrentTransactionWorth =
                            coin.Result.Price * futuresTransactionToAdd.AmountOfCoin;
                        var existingFuturesTransaction = await _futuresTransactionService
                            .GetFuturesTransactionByCoinSymbol(futuresTransactionToAdd.FuturesPortfolioId,
                                futuresTransactionToAdd.CoinSymbol);
                        if (existingFuturesTransaction != null)
                        {
                            float previousPrice = existingFuturesTransaction.BuyingPrice;
                            float previousAmountOfCoins = existingFuturesTransaction.AmountOfCoin;
                            float previousMoneyInput = existingFuturesTransaction.MoneyInput;
                            existingFuturesTransaction.MoneyInput += futuresTransactionToAdd.MoneyInput;
                            existingFuturesTransaction.AmountOfCoin += futuresTransactionToAdd.AmountOfCoin;
                            existingFuturesTransaction.BuyingPrice = (previousMoneyInput +
                                                                   futuresTransactionToAdd.MoneyInput) /
                                (previousAmountOfCoins + futuresTransactionToAdd.AmountOfCoin);
                            var existingFuturesTransactionDto = _mapper.Map<FuturesTransactionDto>(existingFuturesTransaction);
                            await _futuresTransactionService.EditFuturesTransaction(existingFuturesTransactionDto, cancellation);
                            await _futuresTransactionToOpenRepository
                                .RemoveFuturesTransactionToOpen(futuresTransactionToOpen, cancellation);
                            dbTransaction.Commit();
                            return RequestResult.Success();
                        }
                        else
                        {
                            await _futuresTransactionService.AddFuturesTransaction(futuresTransactionToAdd, cancellation);
                            await _futuresTransactionToOpenRepository
                                .RemoveFuturesTransactionToOpen(futuresTransactionToOpen, cancellation);
                            dbTransaction.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    return RequestResult.Failure(TransactionToOpenError.ErrorOpenAwaitingTransactionToOpen);
                }
            }
            return RequestResult.Success();
        }
    }
}
