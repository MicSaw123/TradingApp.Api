using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TradingApp.Application.DataTransferObjects.Spot;
using TradingApp.Application.Repositories.CoinRepository;
using TradingApp.Application.Repositories.DbTransactionRepository;
using TradingApp.Application.Repositories.SpotPortfolioRepository;
using TradingApp.Application.Repositories.SpotTransactionToAdd;
using TradingApp.Application.Services.Interfaces.Database;
using TradingApp.Domain.Errors.TransactionToOpenErrors;
using TradingApp.Domain.Spot;

namespace TradingApp.Application.Repositories.SpotTransactionsToOpen
{
    public class SpotTransactionToOpenRepository : ISpotTransactionToOpenRepository
    {
        private readonly IDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDbTransactionRepository _dbTransaction;
        private readonly ISpotPortfolioRepository _spotPortfolioRepository;
        private readonly ICoinRepository _coinRepository;

        public SpotTransactionToOpenRepository(IDbContext context,
            IMapper mapper, IDbTransactionRepository dbTransaction,
            ISpotPortfolioRepository spotPortfolioRepository, ICoinRepository coinRepository)
        {
            _context = context;
            _mapper = mapper;
            _dbTransaction = dbTransaction;
            _spotPortfolioRepository = spotPortfolioRepository;
            _coinRepository = coinRepository;
        }

        public ICoinRepository CoinRepository { get; }

        public async Task<RequestResult> AddAwaitingTransactionToSpotPortfolio
            (SpotTransactionToOpenDto spotTransactionToOpen,
            CancellationToken cancellation = default)
        {
            var spotTransaction = _mapper.Map<SpotTransactionToOpen>(spotTransactionToOpen);
            using var dbTransaction = _dbTransaction.BeginTransaction();
            try
            {
                await _context.Set<SpotTransactionToOpen>().AddAsync(spotTransaction);
                await _context.SaveChangesAsync(cancellation);
                await _spotPortfolioRepository
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

        public async Task<RequestResult> CancelAwaitingSpotTransaction(int id, int spotPortfolioId,
            CancellationToken cancellation)
        {
            using var dbTransaction = _dbTransaction.BeginTransaction();
            try
            {
                var transaction = await _context.Set<SpotTransactionToOpen>().AsNoTracking().SingleOrDefaultAsync
                    (x => x.Id == id && x.SpotPortfolioId == spotPortfolioId);
                await _spotPortfolioRepository.AddBalance(spotPortfolioId,
                    transaction.MoneyInput, cancellation);
                _context.Set<SpotTransactionToOpen>().Remove(transaction);
                await _context.SaveChangesAsync(cancellation);
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
            var spotTransactionToOpen = _mapper.Map<SpotTransactionToOpen>(spotTransactionToOpenDto);
            using var dbTransaction = _dbTransaction.BeginTransaction();
            try
            {
                var transaction = await _context.Set<SpotTransactionToOpen>().AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == spotTransactionToOpenDto.Id);
                if (transaction.MoneyInput != spotTransactionToOpenDto.MoneyInput)
                {
                    if (transaction.MoneyInput < spotTransactionToOpenDto.MoneyInput)
                    {
                        await _spotPortfolioRepository.
                            SubtractBalance(transaction.SpotPortfolioId,
                            spotTransactionToOpen.MoneyInput - transaction.MoneyInput
                            , cancellation);
                    }
                    else
                    {
                        await _spotPortfolioRepository
                            .AddBalance(transaction.SpotPortfolioId,
                            transaction.MoneyInput - spotTransactionToOpen.MoneyInput, cancellation);
                    }
                }
                _context.Set<SpotTransactionToOpen>().Update(spotTransactionToOpen);
                await _context.SaveChangesAsync(cancellation);
                dbTransaction.Commit();
            }
            catch (Exception ex)
            {
                dbTransaction.Rollback();
                return RequestResult.Failure(TransactionToOpenError.ErrorEditTransactionToOpen);
            }
            return RequestResult.Success();
        }

        public async Task<RequestResult> OpenWaitingSpotTransaction(
            CancellationToken cancellation = default)
        {
            var spotTransactionsToOpen = _context.Set<SpotTransactionToOpen>().ToList();
            foreach (var transaction in spotTransactionsToOpen)
            {
                var coin = await _coinRepository.GetCoinBySymbol(transaction.CoinSymbol);
                using var dbTransaction = _dbTransaction.BeginTransaction();
                try
                {
                    if (transaction.BuyingPrice <= coin.Result.Price)
                    {
                        var spotTransaction = _mapper.Map<SpotTransaction>(transaction);
                        spotTransaction.AmountOfCoin = spotTransaction.MoneyInput / coin.Result.Price;
                        spotTransaction.isActive = true;
                        spotTransaction.BuyingPrice = coin.Result.Price;
                        _context.Set<SpotTransaction>().Add(spotTransaction);
                        await _context.SaveChangesAsync(cancellation);
                        _context.Set<SpotTransactionToOpen>().Remove(transaction);
                        await _context.SaveChangesAsync(cancellation);
                        dbTransaction.Commit();
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
