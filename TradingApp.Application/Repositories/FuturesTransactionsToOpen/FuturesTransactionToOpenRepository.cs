using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TradingApp.Application.DataTransferObjects.Futures;
using TradingApp.Application.Repositories.CoinRepository;
using TradingApp.Application.Repositories.DbTransactionRepository;
using TradingApp.Application.Repositories.FuturesPortfolios;
using TradingApp.Application.Repositories.FuturesTransactionToOpenRepository;
using TradingApp.Application.Services.Interfaces.Database;
using TradingApp.Domain.Errors.TransactionToOpenErrors;
using TradingApp.Domain.Futures;

namespace TradingApp.Application.Repositories.FuturesTransactionsToOpen
{
    public class FuturesTransactionToOpenRepository : IFuturesTransactionToOpenRepository
    {
        private readonly IDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFuturesPortfolioRepository _futuresPortfolioRepository;
        private readonly IDbTransactionRepository _dbTransaction;
        private readonly ICoinRepository _coinRepository;

        public FuturesTransactionToOpenRepository(IDbContext context, IMapper mapper,
            IFuturesPortfolioRepository futuresPortfolioRepository, IDbTransactionRepository dbTransaction,
            ICoinRepository coinRepository)
        {
            _context = context;
            _mapper = mapper;
            _futuresPortfolioRepository = futuresPortfolioRepository;
            _dbTransaction = dbTransaction;
            _coinRepository = coinRepository;
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
                    await _context.Set<FuturesTransactionToOpen>().AddAsync(futuresTransactionToOpen);
                    await _context.SaveChangesAsync(cancellation);
                    await _futuresPortfolioRepository.SubtractBalance(futuresTransactionToOpenDto.FuturesPortfolioId,
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
            var futuresTransaction = await _context.Set<FuturesTransactionToOpen>().FirstOrDefaultAsync(x => x.Id == id);
            if (futuresTransaction is null)
            {
                return RequestResult.Failure(TransactionToOpenError.ErrorCancelTransactionToOpen);
            }
            await _futuresPortfolioRepository.
                AddBalance(futuresPortfolioId, futuresTransaction.MoneyInput, cancellation);
            _context.Set<FuturesTransactionToOpen>().Remove(futuresTransaction);
            await _context.SaveChangesAsync(cancellation);
            return RequestResult.Success();
        }

        public async Task<RequestResult> EditFuturesTransactionToOpen(FuturesTransactionToOpenDto futuresTransactionToOpenDto,
            CancellationToken cancellation)
        {
            var futuresTransactionToOpen = _mapper.Map<FuturesTransactionToOpen>(futuresTransactionToOpenDto);
            if (futuresTransactionToOpen is not null)
            {
                var transaction = await
                    _context.Set<FuturesTransactionToOpen>().AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == futuresTransactionToOpen.Id);
                if (transaction.MoneyInput != futuresTransactionToOpenDto.MoneyInput)
                {
                    if (transaction.MoneyInput > futuresTransactionToOpenDto.MoneyInput)
                    {
                        await _futuresPortfolioRepository.AddBalance(transaction.FuturesPortfolioId,
                            transaction.MoneyInput - futuresTransactionToOpenDto.MoneyInput, cancellation);
                    }
                    else
                    {
                        await _futuresPortfolioRepository.SubtractBalance(transaction.FuturesPortfolioId,
                            futuresTransactionToOpenDto.MoneyInput - transaction.MoneyInput, cancellation);
                    }
                }
                _context.Set<FuturesTransactionToOpen>().Update(futuresTransactionToOpen);
                await _context.SaveChangesAsync(cancellation);
                return RequestResult.Success();
            }
            return RequestResult.Failure(TransactionToOpenError.ErrorEditTransactionToOpen);
        }

        public async Task<RequestResult> OpenFuturesTransactionToOpen(CancellationToken cancellation)
        {
            var transactionsToOpen = _context.Set<FuturesTransactionToOpen>().ToList();
            foreach (var transaction in transactionsToOpen)
            {
                var coin = await _coinRepository.GetCoinBySymbol(transaction.CoinSymbol);
                using var dbTransaction = _dbTransaction.BeginTransaction();
                try
                {
                    if (transaction.BuyingPrice >= coin.Result.Price)
                    {
                        var futuresTransaction = _mapper.Map<FuturesTransaction>(transaction);
                        futuresTransaction.isActive = true;
                        futuresTransaction.AmountOfCoin = futuresTransaction.MoneyInput / coin.Result.Price;
                        futuresTransaction.BuyingPrice = coin.Result.Price;
                        _context.Set<FuturesTransaction>().Add(futuresTransaction);
                        _context.Set<FuturesTransactionToOpen>().Remove(transaction);
                        await _context.SaveChangesAsync(cancellation);
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
