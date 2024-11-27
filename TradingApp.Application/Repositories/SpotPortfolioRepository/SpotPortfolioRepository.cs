using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TradingApp.Application.DataTransferObjects.Portfolio;
using TradingApp.Application.Repositories.DbTransactionRepository;
using TradingApp.Application.Services.Interfaces.Database;
using TradingApp.Domain.Errors.Errors.SpotPortfolioErrors;
using TradingApp.Domain.Portfolio;
using TradingApp.Domain.Spot;

namespace TradingApp.Application.Repositories.SpotPortfolioRepository
{
    public class SpotPortfolioRepository : ISpotPortfolioRepository
    {
        private readonly IDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDbTransactionRepository _dbTransaction;

        public SpotPortfolioRepository(IDbContext context, IMapper mapper,
            IDbTransactionRepository dbTransaction)
        {
            _context = context;
            _mapper = mapper;
            _dbTransaction = dbTransaction;
        }

        public async Task<RequestResult> AddBalance(int id, float amountToAdd,
            CancellationToken cancellation = default)
        {
            var spotPortfolioDto = await GetSpotPortfolioById(id);
            spotPortfolioDto.Result.Balance += amountToAdd;
            var spotPortfolio = _mapper.Map<SpotPortfolio>(spotPortfolioDto.Result);
            _context.Set<SpotPortfolio>().Update(spotPortfolio);
            await _context.SaveChangesAsync(cancellation);
            if (spotPortfolio is null)
            {
                return RequestResult.Failure(PortfolioError.ErrorAddFunds);
            }
            return RequestResult.Success();
        }

        public async Task<SpotPortfolio> ChangeProfitsAndBalance(SpotPortfolio spotPortfolio,
            CancellationToken cancellation = default)
        {
            var transactions = _context.Set<SpotTransaction>()
                .Where(x => x.SpotPortfolioId == spotPortfolio.Id);
            foreach (var transaction in transactions)
            {
                spotPortfolio.DailyProfit += transaction.TransactionProfit;
                spotPortfolio.WeeklyProfit += transaction.TransactionProfit;
                spotPortfolio.MonthlyProfit += transaction.TransactionProfit;
                spotPortfolio.Balance += transaction.TransactionProfit +
                    transaction.MoneyInput;
            }
            return spotPortfolio;
        }

        public async Task<RequestResult> CalculateProfits(CancellationToken cancellation)
        {
            var spotPortfolios = _context.Set<SpotPortfolio>();
            foreach (var spotPortfolio in spotPortfolios)
            {
                using var dbTransaction = _dbTransaction.BeginTransaction();
                try
                {
                    await ChangeProfitsAndBalance(spotPortfolio, cancellation);
                    _context.Set<SpotPortfolio>().Update(spotPortfolio);
                    await _context.SaveChangesAsync(cancellation);
                    dbTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    return RequestResult.Failure(PortfolioError.ErrorCalculatePortfolioProfits);
                }
            }
            return RequestResult.Success();
        }

        public async Task<RequestResult<SpotPortfolioDto>> GetSpotPortfolioById(int id)
        {
            var spotPortfolio = await _context.Set<SpotPortfolio>().AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            var spotPortfolioDto = _mapper.Map<SpotPortfolioDto>(spotPortfolio);
            if (spotPortfolio is null)
            {
                return RequestResult<SpotPortfolioDto>.Failure(PortfolioError.ErrorGetPortfolioById);
            }
            return RequestResult<SpotPortfolioDto>.Success(spotPortfolioDto);
        }

        public async Task<RequestResult<SpotPortfolioDto>> GetSpotPortfolioByUserId(string userId)
        {
            var portfolio = await _context.Set<Portfolio>().AsNoTracking().Include(x => x.SpotPortfolio)
                .SingleOrDefaultAsync(x => x.TradingAppUser.Id == userId);
            if (portfolio is null)
            {
                return RequestResult<SpotPortfolioDto>.Failure(PortfolioError.ErrorGetPortfolioByUserId);
            }
            var spotPortfolioDto = _mapper.Map<SpotPortfolioDto>(portfolio.SpotPortfolio);
            return RequestResult<SpotPortfolioDto>.Success(spotPortfolioDto);
        }

        public async Task<RequestResult> SubtractBalance(int id, float amountToSubtract,
            CancellationToken cancellation = default)
        {
            var spotPortfolioDto = await GetSpotPortfolioById(id);
            spotPortfolioDto.Result.Balance -= amountToSubtract;
            if (spotPortfolioDto.Result.Balance < 0)
            {
                return RequestResult.Failure(PortfolioError.NonSufficientFunds);
            }
            var spotPortfolio = _mapper.Map<SpotPortfolio>(spotPortfolioDto.Result);
            _context.Set<SpotPortfolio>().Update(spotPortfolio);
            await _context.SaveChangesAsync(cancellation);
            return RequestResult.Success();
        }
    }
}
