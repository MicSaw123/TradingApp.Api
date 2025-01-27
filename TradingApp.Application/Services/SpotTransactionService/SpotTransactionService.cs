using AutoMapper;
using TradingApp.Application.DataTransferObjects.Transaction;
using TradingApp.Application.Repositories.DbTransactionRepository;
using TradingApp.Application.Repositories.SpotTransactionRepository;
using TradingApp.Application.Services.CoinService;
using TradingApp.Application.Services.SpotPortfolioService;
using TradingApp.Domain.Errors.Errors.TransactionErrors;
using TradingApp.Domain.Spot;

namespace TradingApp.Application.Services.SpotTransactionService
{
    public class SpotTransactionService : ISpotTransactionService
    {
        private readonly ISpotTransactionRepository _spotTransactionRepository;
        private readonly ISpotPortfolioService _spotPortfolioService;
        private readonly ICoinService _coinService;
        private readonly IMapper _mapper;
        private readonly IDbTransactionRepository _dbTransaction;

        public SpotTransactionService(ISpotTransactionRepository spotTransactionRepository,
            ISpotPortfolioService spotPortfolioService, ICoinService coinService, IMapper mapper,
            IDbTransactionRepository dbTransaction)
        {
            _spotTransactionRepository = spotTransactionRepository;
            _spotPortfolioService = spotPortfolioService;
            _coinService = coinService;
            _mapper = mapper;
            _dbTransaction = dbTransaction;
        }

        public async Task AddSpotTransaction(SpotTransaction spotTransaction, CancellationToken cancellation)
        {
            await _spotTransactionRepository.AddSpotTransaction(spotTransaction, cancellation);
        }

        public async Task<RequestResult> CalculateSpotTransactionProfit(CancellationToken cancellation)
        {
            var spotPortfolios = await _spotPortfolioService.GetSpotPortfolios();
            foreach (var spotPortfolio in spotPortfolios.Result)
            {
                float portfolioProfit = 0;
                var spotTransactions = await _spotTransactionRepository.GetActiveSpotTransactionsByPortfolioId(spotPortfolio.Id);
                if (spotTransactions is not null)
                {
                    foreach (var spotTransaction in spotTransactions)
                    {
                        var coin = await _coinService.GetCoinBySymbol(spotTransaction.CoinSymbol);
                        spotTransaction.TransactionProfit = (spotTransaction.AmountOfCoin * coin.Result.Price)
                        - spotTransaction.MoneyInput;
                        spotTransaction.CurrentValue = coin.Result.Price * spotTransaction.AmountOfCoin;
                        portfolioProfit += spotTransaction.TransactionProfit;
                    }
                    spotPortfolio.DailyProfit += portfolioProfit;
                    spotPortfolio.WeeklyProfit += portfolioProfit;
                    spotPortfolio.MonthlyProfit += portfolioProfit;
                    spotPortfolio.Balance += portfolioProfit;
                }
                else
                {
                    continue;
                }
                await _spotTransactionRepository.UpdateSpotTransactionRange(spotTransactions.ToList(), cancellation);
            }
            await _spotPortfolioService.EditSpotPortfolios(spotPortfolios.Result.ToList(), cancellation);
            return RequestResult.Success();
        }

        public async Task<RequestResult> CloseExistingSpotTransactions(CancellationToken cancellation)
        {
            var transactions = await _spotTransactionRepository.GetSpotTransactionsWithSellingPrice();
            foreach (var transaction in transactions)
            {
                var coin = await _coinService.GetCoinBySymbol(transaction.CoinSymbol);
                if (transaction.SellingPrice >= coin.Result.Price)
                {
                    using var dbTransaction = _dbTransaction.BeginTransaction();
                    try
                    {
                        transaction.IsActive = false;
                        transaction.ClosingTransactionDate = DateOnly.FromDateTime(DateTime.Now);
                        transaction.TransactionProfit =
                            transaction.AmountOfCoin * coin.Result.Price - transaction.MoneyInput;
                        var spotPortfolio = await _spotPortfolioService.GetSpotPortfolioById(transaction.SpotPortfolioId);
                        await _spotPortfolioService.EditSpotPortfolio(spotPortfolio, cancellation);
                        await _spotTransactionRepository.UpdateSpotTransaction(transaction, cancellation);
                        dbTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        return RequestResult.Failure(TransactionError.ErrorCloseTransaction);
                    }
                }
            }
            return RequestResult.Success();
        }

        public async Task<RequestResult> CloseExistingTransactionsManually(SpotTransactionDto spotTransactionDto, CancellationToken cancellation)
        {
            var spotTransaction = _mapper.Map<SpotTransaction>(spotTransactionDto);
            var transaction = await _spotTransactionRepository.GetSpotTransactionById(spotTransaction.Id);
            if (transaction is not null && transaction.IsActive is true)
            {
                var coin = await _coinService.GetCoinBySymbol(transaction.CoinSymbol);
                transaction.SellingPrice = coin.Result.Price;
                transaction.TransactionProfit =
                    transaction.SellingPrice * transaction.AmountOfCoin - transaction.MoneyInput;
                transaction.IsActive = false;
                using var dbTransaction = _dbTransaction.BeginTransaction();
                try
                {
                    await _spotTransactionRepository.UpdateSpotTransaction(spotTransaction, cancellation);
                    var portfolio = await _spotPortfolioService.GetSpotPortfolioById(transaction.SpotPortfolioId);
                    portfolio.Balance += transaction.TransactionProfit + transaction.MoneyInput;
                    portfolio.DailyProfit += transaction.TransactionProfit;
                    portfolio.WeeklyProfit += transaction.TransactionProfit;
                    portfolio.MonthlyProfit += transaction.TransactionProfit;
                    await _spotPortfolioService.EditSpotPortfolio(portfolio, cancellation);
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

        public async Task<RequestResult> EditSpotTransaction(SpotTransactionDto spotTransactionDto, CancellationToken cancellation)
        {
            var spotTransaction = _mapper.Map<SpotTransaction>(spotTransactionDto);
            await _spotTransactionRepository.UpdateSpotTransaction(spotTransaction, cancellation);
            if (cancellation.IsCancellationRequested)
            {
                return RequestResult.Failure(TransactionError.ErrorEditTransaction);
            }
            return RequestResult.Success();
        }

        public async Task<RequestResult<IEnumerable<SpotTransaction>>> GetActiveTransactionsByPortfolioId(int poiPortfolioId)
        {
            var result = await _spotTransactionRepository.GetActiveSpotTransactionsByPortfolioId(poiPortfolioId);
            if (result is null)
            {
                return RequestResult<IEnumerable<SpotTransaction>>
                    .Failure(TransactionError.ErrorGetTransactionsByPortfolioId);
            }
            return RequestResult<IEnumerable<SpotTransaction>>.Success(result);
        }

        public async Task<SpotTransaction> GetExistingSpotTransactionWithSpecifiedCoinSymbol(int portfolioId, string coinSymbol)
        {
            var result = await _spotTransactionRepository
                .GetExistingSpotTransactionWithSpecifiedCoinSymbol(portfolioId, coinSymbol);
            return result;
        }
    }
}
