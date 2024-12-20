using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TradingApp.Application.DataTransferObjects.Transaction;
using TradingApp.Application.Repositories.CoinRepository;
using TradingApp.Application.Repositories.DbTransactionRepository;
using TradingApp.Application.Repositories.SpotPortfolioRepository;
using TradingApp.Application.Services.Interfaces.Database;
using TradingApp.Domain.Errors.Errors.TransactionErrors;
using TradingApp.Domain.Spot;

namespace TradingApp.Application.Repositories.SpotTransactionRepository
{
    public class SpotTransactionRepository : ISpotTransactionRepository
    {
        private readonly IDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISpotPortfolioRepository _spotPortfolioRepository;
        private readonly IDbTransactionRepository _dbTransaction;
        private readonly ICoinRepository _coinRepository;

        public SpotTransactionRepository(IDbContext context, IMapper mapper,
            ISpotPortfolioRepository spotPortfolioRepository,
            IDbTransactionRepository dbTransaction, ICoinRepository coinRepository)
        {
            _context = context;
            _mapper = mapper;
            _spotPortfolioRepository = spotPortfolioRepository;
            _dbTransaction = dbTransaction;
            _coinRepository = coinRepository;
        }

        public async Task<RequestResult> CalculateSpotTransactionProfit
            (CancellationToken cancellation)
        {
            var spotPortfolios = _context.Set<SpotPortfolio>();
            foreach (var spotPortfolio in spotPortfolios)
            {
                var spotTransactions = _context.Set<SpotTransaction>()
                    .Where(x => x.SpotPortfolioId == spotPortfolio.Id);
                foreach (var transaction in spotTransactions)
                {

                    var coin = await _coinRepository.GetCoinBySymbol(transaction.CoinSymbol);
                    transaction.TransactionProfit = transaction.AmountOfCoin * coin.Result.Price
                        - transaction.MoneyInput;
                    spotPortfolio.Balance += transaction.TransactionProfit;
                    spotPortfolio.DailyProfit += transaction.TransactionProfit;
                    spotPortfolio.WeeklyProfit += transaction.TransactionProfit;
                    spotPortfolio.MonthlyProfit += transaction.TransactionProfit;
                    using var dbTransaction = _dbTransaction.BeginTransaction();
                    try
                    {
                        _context.Set<SpotTransaction>().Update(transaction);
                        await _context.SaveChangesAsync(cancellation);
                        dbTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        return RequestResult.Failure(TransactionError.ErrorCalculateTransactionProfits);
                    }
                }
            }
            return RequestResult.Success();
        }

        public async Task<RequestResult> CloseExistingTransaction(CancellationToken cancellation = default)
        {
            var transactions = _context.Set<SpotTransaction>().Where(x => x.SellingPrice != 0);
            foreach (var transaction in transactions)
            {
                var coin = await _coinRepository.GetCoinBySymbol(transaction.CoinSymbol);
                if (transaction.SellingPrice >= coin.Result.Price)
                {
                    using var dbTransaction = _dbTransaction.BeginTransaction();
                    try
                    {
                        transaction.isActive = false;
                        transaction.TransactionProfit =
                            transaction.AmountOfCoin * coin.Result.Price - transaction.MoneyInput;
                        var spotPortfolioDto = await
                            _spotPortfolioRepository.GetSpotPortfolioById(transaction.SpotPortfolioId);
                        var spotPortfolio = _mapper.Map<SpotPortfolio>(spotPortfolioDto.Result);
                        _context.Set<SpotPortfolio>().Update(spotPortfolio);
                        await _context.SaveChangesAsync(cancellation);
                        _context.Set<SpotTransaction>().Update(transaction);
                        await _context.SaveChangesAsync(cancellation);
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

        public async Task<RequestResult> CloseSpotTransactionManually(SpotTransactionDto spotTransactionDto,
            CancellationToken cancellation)
        {
            var spotTransaction = _mapper.Map<SpotTransaction>(spotTransactionDto);
            var transaction = await _context.Set<SpotTransaction>()
                .SingleOrDefaultAsync(x => x.Id == spotTransaction.Id);
            if (transaction is not null && transaction.isActive is true)
            {
                var coin = await _coinRepository.GetCoinBySymbol(transaction.CoinSymbol);
                transaction.SellingPrice = coin.Result.Price;
                transaction.TransactionProfit =
                    transaction.SellingPrice * transaction.AmountOfCoin - transaction.MoneyInput;
                transaction.isActive = false;
                using var dbTransaction = _dbTransaction.BeginTransaction();
                try
                {
                    _context.Set<SpotTransaction>().Update(transaction);
                    await _context.SaveChangesAsync(cancellation);
                    var portfolio = await _context.Set<SpotPortfolio>()
                        .SingleOrDefaultAsync(x => x.Id == transaction.SpotPortfolioId);
                    portfolio.Balance += transaction.TransactionProfit + transaction.MoneyInput;
                    portfolio.DailyProfit += transaction.TransactionProfit;
                    portfolio.WeeklyProfit += transaction.TransactionProfit;
                    portfolio.MonthlyProfit += transaction.TransactionProfit;
                    await _context.SaveChangesAsync(cancellation);
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

        public async Task<RequestResult> EditSpotTransaction(SpotTransactionDto spotTransactionDto,
            CancellationToken cancellation)
        {
            var spotTransaction = _mapper.Map<SpotTransaction>(spotTransactionDto);
            using var dbTransaction = _dbTransaction.BeginTransaction();
            try
            {
                _context.Set<SpotTransaction>().Update(spotTransaction);
                await _context.SaveChangesAsync(cancellation);
                dbTransaction.Commit();
            }
            catch (Exception ex)
            {
                dbTransaction.Rollback();
                return RequestResult.Failure(TransactionError.ErrorEditTransaction);
            }
            return RequestResult.Success();
        }
    }
}
