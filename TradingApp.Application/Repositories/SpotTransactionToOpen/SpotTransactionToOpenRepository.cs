using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using TradingApp.Application.DataTransferObjects.Coin;
using TradingApp.Application.DataTransferObjects.Spot;
using TradingApp.Application.Repositories.DbTransactionRepository;
using TradingApp.Application.Repositories.SpotPortfolioRepository;
using TradingApp.Application.Services.Interfaces.Database;
using TradingApp.Domain.Errors.TransactionToOpenErrors;
using TradingApp.Domain.Spot;

namespace TradingApp.Application.Repositories.SpotTransactionToAdd
{
    public class SpotTransactionToOpenRepository : ISpotTransactionToOpenRepository
    {
        private readonly IDbContext _context;
        private readonly HttpClient _http;
        private readonly IMapper _mapper;
        private readonly IDbTransactionRepository _dbTransaction;
        private readonly ISpotPortfolioRepository _spotPortfolioRepository;
        private string baseApiAddress = "https://api.binance.com/api/v3/ticker/price?symbol=";

        public SpotTransactionToOpenRepository(IDbContext context, HttpClient http,
            IMapper mapper, IDbTransactionRepository dbTransaction,
            ISpotPortfolioRepository spotPortfolioRepository)
        {
            _context = context;
            _http = http;
            _mapper = mapper;
            _dbTransaction = dbTransaction;
            _spotPortfolioRepository = spotPortfolioRepository;
        }

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
                            SubtractBalance(transaction.SpotPortfolioId, spotTransactionToOpen.MoneyInput - transaction.MoneyInput
                            , cancellation);
                    }
                    else
                    {
                        await _spotPortfolioRepository
                            .AddBalance(transaction.SpotPortfolioId, transaction.MoneyInput - spotTransactionToOpen.MoneyInput, cancellation);
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
                var coin = await _http.GetFromJsonAsync<CoinDto>
                    (baseApiAddress + transaction.CoinSymbol);
                using var dbTransaction = _dbTransaction.BeginTransaction();
                try
                {
                    if (transaction.BuyingPrice >= coin?.Price)
                    {
                        var spotTransaction = _mapper.Map<SpotTransaction>(transaction);
                        spotTransaction.AmountOfCoin = spotTransaction.MoneyInput / coin.Price;
                        spotTransaction.isActive = true;
                        spotTransaction.BuyingPrice = coin.Price;
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
