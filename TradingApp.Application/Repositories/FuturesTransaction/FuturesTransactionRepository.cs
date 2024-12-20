using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TradingApp.Application.DataTransferObjects.Transaction;
using TradingApp.Application.Repositories.CoinRepository;
using TradingApp.Application.Repositories.DbTransactionRepository;
using TradingApp.Application.Repositories.FuturesPortfolios;
using TradingApp.Application.Services.Interfaces.Database;
using TradingApp.Domain.Errors.Errors.TransactionErrors;
using TradingApp.Domain.Futures;

namespace TradingApp.Application.Repositories.TransactionRepository.FuturesTransactionRepository
{
    public class FuturesTransactionRepository : IFuturesTransactionRepository
    {
        private readonly IDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDbTransactionRepository _dbTransaction;
        private readonly IFuturesPortfolioRepository _futuresPortfolioRepository;
        private readonly ICoinRepository _coinRepository;

        public FuturesTransactionRepository(IDbContext context, IMapper mapper, IDbTransactionRepository dbTransaction,
            IFuturesPortfolioRepository futuresPortfolioRepository, ICoinRepository coinRepository)
        {
            _context = context;
            _mapper = mapper;
            _dbTransaction = dbTransaction;
            _futuresPortfolioRepository = futuresPortfolioRepository;
            _coinRepository = coinRepository;
        }

        public async Task<RequestResult> CalculateTransactionsProfits(CancellationToken cancellation)
        {
            var futuresTransactions = _context.Set<FuturesTransaction>().ToList();
            foreach (var futuresTransaction in futuresTransactions)
            {
                using var dbTransaction = _dbTransaction.BeginTransaction();
                try
                {
                    var coin = await _coinRepository.GetCoinBySymbol(futuresTransaction.CoinSymbol);
                    futuresTransaction.TransactionProfit =
                        (futuresTransaction.AmountOfCoin * coin.Result.Price) - futuresTransaction.MoneyInput;
                    if (futuresTransaction.TransactionProfit < 0)
                    {
                        await _futuresPortfolioRepository.SubtractBalance(futuresTransaction.FuturesPortfolioId,
                            futuresTransaction.TransactionProfit, cancellation);
                    }
                    else if (futuresTransaction.TransactionProfit > 0)
                    {
                        await _futuresPortfolioRepository.AddBalance(futuresTransaction.FuturesPortfolioId,
                            futuresTransaction.TransactionProfit, cancellation);
                    }
                    _context.Set<FuturesTransaction>().Update(futuresTransaction);
                    await _context.SaveChangesAsync(cancellation);
                    dbTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                }
            }
            return RequestResult.Success();
        }

        public async Task<RequestResult> CloseFuturesTransaction(int id, int portfolioId, CancellationToken cancellation)
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
                    transactionToClose.isActive = false;
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
        }

        public async Task<RequestResult> EditFuturesTransaction(FuturesTransactionDto futuresTransactionDto,
            CancellationToken cancellation)
        {
            var transactionToEdit = _mapper.Map<FuturesTransaction>(futuresTransactionDto);
            if (transactionToEdit != null)
            {
                _context.Set<FuturesTransaction>().Update(transactionToEdit);
                await _context.SaveChangesAsync(cancellation);
                return RequestResult.Success();
            }
            return RequestResult.Failure(TransactionError.ErrorEditTransaction);
        }

        public async Task<RequestResult<IEnumerable<FuturesTransactionDto>>>
            GetFuturesTransactionByPortfolioId(int portfolioId, CancellationToken cancellation)
        {
            var transactions = _context.Set<FuturesTransaction>().Where(x => x.FuturesPortfolioId == portfolioId)
                .AsEnumerable();
            var transactionDtos = _mapper.Map<IEnumerable<FuturesTransactionDto>>(transactions);
            if (transactions is null)
            {
                return RequestResult<IEnumerable<FuturesTransactionDto>>
                    .Failure(TransactionError.ErrorGetTransactionsByPortfolioId);
            }
            return RequestResult<IEnumerable<FuturesTransactionDto>>.Success(transactionDtos);
        }
    }
}
