
using AutoMapper;
using TradingApp.Application.DataTransferObjects.Spot;
using TradingApp.Application.DataTransferObjects.Transaction;
using TradingApp.Application.Repositories.DbTransactionRepository;
using TradingApp.Application.Repositories.SpotTransactionToAdd;
using TradingApp.Application.Services.CoinService;
using TradingApp.Application.Services.SpotPortfolioService;
using TradingApp.Application.Services.SpotTransactionService;
using TradingApp.Domain.Errors.TransactionToOpenErrors;
using TradingApp.Domain.Spot;

namespace TradingApp.Application.Services.SpotTransactionToOpenService
{
    public class SpotTransactionToOpenService : ISpotTransactionToOpenService
    {
        private readonly ISpotTransactionToOpenRepository _spotTransactionToOpenRepository;
        private readonly IMapper _mapper;
        private readonly IDbTransactionRepository _dbTransaction;
        private readonly ISpotPortfolioService _spotPortfolioService;
        private readonly ICoinService _coinService;
        private readonly ISpotTransactionService _spotTransactionService;

        public SpotTransactionToOpenService(ISpotTransactionToOpenRepository
            spotTransactionToOpenRepository, IMapper mapper, IDbTransactionRepository dbTransaction,
            ISpotPortfolioService spotPortfolioService, ICoinService coinService, ISpotTransactionService spotTransactionService)
        {
            _spotTransactionToOpenRepository = spotTransactionToOpenRepository;
            _mapper = mapper;
            _dbTransaction = dbTransaction;
            _spotPortfolioService = spotPortfolioService;
            _coinService = coinService;
            _spotTransactionService = spotTransactionService;
        }

        public async Task<RequestResult> AddAwaitingTransactionToSpotPortfolio
            (SpotTransactionToOpenDto spotTransactionToOpen,
            CancellationToken cancellation = default)
        {
            var spotTransaction = _mapper.Map<SpotTransactionToOpen>(spotTransactionToOpen);
            using var dbTransaction = _dbTransaction.BeginTransaction();
            try
            {
                await _spotTransactionToOpenRepository.AddSpotTransactionToOpen(spotTransaction, cancellation);
                await _spotPortfolioService
                    .SubtractBalance(spotTransaction.SpotPortfolioId, spotTransaction.MoneyInput, cancellation);
                dbTransaction.Commit();
            }
            catch (Exception ex)
            {
                dbTransaction.Rollback();
                return RequestResult.Failure(TransactionToOpenError.ErrorAddTransactionToOpen);
            }
            return RequestResult.Success();
        }

        public async Task<RequestResult> CancelAwaitingSpotTransaction(int id,
            CancellationToken cancellation)
        {
            using var dbTransaction = _dbTransaction.BeginTransaction();
            try
            {
                var transaction = await _spotTransactionToOpenRepository.GetSpotTransactionToOpenById(id);
                await _spotPortfolioService.AddBalance(transaction.SpotPortfolioId,
                    transaction.MoneyInput, cancellation);
                await _spotTransactionToOpenRepository.RemoveSpotTransactionToOpen(transaction, cancellation);
                dbTransaction.Commit();
            }
            catch (Exception ex)
            {
                dbTransaction.Rollback();
                return RequestResult.Failure(TransactionToOpenError.ErrorCancelTransactionToOpen);
            }
            return RequestResult.Success();
        }

        public async Task<RequestResult> EditAwaitingSpotTransaction(SpotTransactionToOpenDto spotTransactionToOpenDto,
            CancellationToken cancellation)
        {
            using var dbTransaction = _dbTransaction.BeginTransaction();
            try
            {
                var transaction = await _spotTransactionToOpenRepository.GetSpotTransactionToOpenById(spotTransactionToOpenDto.Id);
                if (transaction.MoneyInput != spotTransactionToOpenDto.MoneyInput)
                {
                    if (transaction.MoneyInput < spotTransactionToOpenDto.MoneyInput)
                    {
                        await _spotPortfolioService.
                            SubtractBalance(transaction.SpotPortfolioId,
                            spotTransactionToOpenDto.MoneyInput - transaction.MoneyInput, cancellation);
                    }
                    else
                    {
                        await _spotPortfolioService
                            .AddBalance(transaction.SpotPortfolioId,
                            transaction.MoneyInput - spotTransactionToOpenDto.MoneyInput, cancellation);
                    }
                }
                transaction.BuyingPrice = spotTransactionToOpenDto.BuyingPrice;
                transaction.MoneyInput = spotTransactionToOpenDto.MoneyInput;
                await _spotTransactionToOpenRepository.EditSpotTransactionToOpen(transaction, cancellation);
                dbTransaction.Commit();
            }
            catch (Exception ex)
            {
                dbTransaction.Rollback();
                return RequestResult.Failure(TransactionToOpenError.ErrorEditTransactionToOpen);
            }
            return RequestResult.Success();
        }

        public async Task<RequestResult> OpenWaitingSpotTransaction(CancellationToken cancellation)
        {
            var spotTransactionsToOpen = await _spotTransactionToOpenRepository.GetSpotTransactionsToOpen();
            foreach (var spotTransactionToOpen in spotTransactionsToOpen)
            {
                var coin = await _coinService.GetCoinBySymbol(spotTransactionToOpen.CoinSymbol);
                using var dbTransaction = _dbTransaction.BeginTransaction();
                try
                {
                    if (spotTransactionToOpen.BuyingPrice >= coin.Result.Price)
                    {
                        var spotTransactionToAdd = _mapper.Map<SpotTransaction>(spotTransactionToOpen);
                        spotTransactionToAdd.AmountOfCoin = spotTransactionToAdd.MoneyInput / coin.Result.Price;
                        spotTransactionToAdd.IsActive = true;
                        spotTransactionToAdd.BuyingPrice = coin.Result.Price;
                        spotTransactionToAdd.CurrentValue = coin.Result.Price * spotTransactionToAdd.AmountOfCoin;
                        var existingSpotTransaction = await _spotTransactionService
                            .GetExistingSpotTransactionWithSpecifiedCoinSymbol(spotTransactionToAdd.SpotPortfolioId, spotTransactionToAdd.CoinSymbol);
                        if (existingSpotTransaction != null)
                        {
                            float previousPrice = existingSpotTransaction.BuyingPrice;
                            float previousAmountOfCoins = existingSpotTransaction.AmountOfCoin;
                            float previousMoneyInput = existingSpotTransaction.MoneyInput;
                            existingSpotTransaction.MoneyInput += spotTransactionToAdd.MoneyInput;
                            existingSpotTransaction.AmountOfCoin += spotTransactionToAdd.AmountOfCoin;
                            existingSpotTransaction.BuyingPrice = (previousMoneyInput + spotTransactionToAdd.MoneyInput) /
                                (previousAmountOfCoins + spotTransactionToAdd.AmountOfCoin);
                            var existingSpotTransactionDto = _mapper.Map<SpotTransactionDto>(existingSpotTransaction);
                            await _spotTransactionService.EditSpotTransaction(existingSpotTransactionDto, cancellation);
                            await _spotTransactionToOpenRepository.RemoveSpotTransactionToOpen(spotTransactionToOpen, cancellation);
                            dbTransaction.Commit();
                            return RequestResult.Success();
                        }
                        else
                        {
                            await _spotTransactionService.AddSpotTransaction(spotTransactionToAdd, cancellation);
                            await _spotTransactionToOpenRepository.RemoveSpotTransactionToOpen(spotTransactionToOpen, cancellation);
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
