using Microsoft.EntityFrameworkCore;
using TradingApp.Application.Services.Interfaces.Database;
using TradingApp.Domain.Futures;

namespace TradingApp.Application.Repositories.TransactionRepository.FuturesTransactionRepository
{
    public class FuturesTransactionRepository : IFuturesTransactionRepository
    {
        private readonly IDbContext _context;

        public FuturesTransactionRepository(IDbContext context)
        {
            _context = context;
        }

        /*public async Task<RequestResult> CalculateTransactionsProfits(CancellationToken cancellation)
        {
            var futuresPortfolios = await _futuresPortfolioRepository.GetFuturesPortfolios();
            foreach (var futuresPortfolio in futuresPortfolios)
            {
                float totalPortfolioProfit = 0;
                var futuresTransactionDtos = await GetActiveFuturesTransactionByPortfolioId(futuresPortfolio.Id, cancellation);
                var futuresTransactions = _mapper.Map<List<FuturesTransaction>>(futuresTransactionDtos);
                foreach (var futuresTransaction in futuresTransactions)
                {
                    var coin = await _coinRepository.GetCoinBySymbol(futuresTransaction.CoinSymbol);
                    futuresTransaction.TransactionProfit += (futuresTransaction.AmountOfCoin * coin.Result.Price)
                        - (futuresTransaction.AmountOfCoin * futuresTransaction.BuyingPrice);
                    totalPortfolioProfit += futuresTransaction.TransactionProfit;
                }
                futuresPortfolio.DailyProfit += totalPortfolioProfit;
                futuresPortfolio.WeeklyProfit += totalPortfolioProfit;
                futuresPortfolio.MonthlyProfit += totalPortfolioProfit;
                await UpdateFuturesTransactions(futuresTransactions, cancellation);
            }
            await _futuresPortfolioRepository.UpdateFuturesPortfolios(futuresPortfolios, cancellation);
            return RequestResult.Success();
        }*/
        /*public async Task<RequestResult> CloseFuturesTransaction(int id, int portfolioId, CancellationToken cancellation)
        {
            var transactionToClose = await _context.Set<FuturesTransaction>()
                .FirstOrDefaultAsync(x => x.Id == id && x.FuturesPortfolioId == portfolioId);
            if (transactionToClose != null)
            {
                using var dbTransaction = _dbTransaction.BeginTransaction();
                try
                {
                    var coin = await _coinRepository.GetCoinBySymbol(transactionToClose.CoinSymbol);
                    transactionToClose.TransactionProfit =
                        (transactionToClose.AmountOfCoin * coin.Result.Price) - transactionToClose.MoneyInput;
                    await _futuresPortfolioRepository
                        .AddBalance(transactionToClose.FuturesPortfolioId,
                        transactionToClose.TransactionProfit + transactionToClose.MoneyInput, cancellation);
                    transactionToClose.IsActive = false;
                    _context.Set<FuturesTransaction>().Update(transactionToClose);
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
        }*/


        public async Task UpdateFuturesTransaction(FuturesTransaction transaction, CancellationToken cancellation)
        {
            _context.Set<FuturesTransaction>().Update(transaction);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task UpdateFuturesTransactionRange(List<FuturesTransaction> futuresTransactions,
            CancellationToken cancellation)
        {
            _context.Set<FuturesTransaction>().UpdateRange(futuresTransactions);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task<IEnumerable<FuturesTransaction>> GetActiveFuturesTransactionsByPortfolioId(int portfolioId)
        {
            var futuresTransactions = _context.Set<FuturesTransaction>().Where(x => x.FuturesPortfolioId == portfolioId
            && x.IsActive == true);
            return futuresTransactions;
        }

        public async Task<FuturesTransaction> GetActiveFuturesTransactionById(int id)
        {
            var futuresTransaction = await _context.Set<FuturesTransaction>().FirstOrDefaultAsync(x => x.Id == id && x.IsActive == true);
            return futuresTransaction;
        }

        public async Task AddFuturesTransaction(FuturesTransaction futuresTransaction, CancellationToken cancellation)
        {
            await _context.Set<FuturesTransaction>().AddAsync(futuresTransaction);
            await _context.SaveChangesAsync(cancellation);
        }

    }
}
