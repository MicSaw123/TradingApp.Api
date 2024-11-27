using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TradingApp.Application.Repositories.DbTransactionRepository;
using TradingApp.Application.Services.Interfaces.Database;
using TradingApp.Database.TradingAppUsers;
using TradingApp.Domain.Futures;
using TradingApp.Domain.Portfolio;
using TradingApp.Domain.Spot;

namespace TradingApp.Application.Services.IdentityService
{
    public class IdentityService : UserManager<TradingAppUser>
    {
        private readonly IDbContext _context;
        private readonly IDbTransactionRepository _dbTransactionRepository;

        public IdentityService(IUserStore<TradingAppUser> store, IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<TradingAppUser> passwordHasher, IEnumerable<IUserValidator<TradingAppUser>> userValidators,
        IEnumerable<IPasswordValidator<TradingAppUser>> passwordValidators, ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<TradingAppUser>> logger, IDbContext context,
        IDbTransactionRepository dbTransactionRepository) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _context = context;
            _dbTransactionRepository = dbTransactionRepository;
        }

        public override async Task<IdentityResult> CreateAsync(TradingAppUser user)
        {
            CancellationToken cancellation = default;
            var portfolio = new Portfolio();
            var spotPortfolio = new SpotPortfolio();
            var futuresPortfolio = new FuturesPortfolio();
            await base.CreateAsync(user);
            using var transaction = _dbTransactionRepository.BeginTransaction();
            try
            {
                await _context.Set<SpotPortfolio>().AddAsync(spotPortfolio);
                await _context.SaveChangesAsync(cancellation);
                await _context.Set<FuturesPortfolio>().AddAsync(futuresPortfolio);
                await _context.SaveChangesAsync(cancellation);
                portfolio.FuturesPortfolioId = futuresPortfolio.Id;
                portfolio.SpotPortfolioId = spotPortfolio.Id;
                portfolio.TradingAppUserId = user.Id;
                await _context.Set<Portfolio>().AddAsync(portfolio);
                await _context.SaveChangesAsync(cancellation);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
            return IdentityResult.Success;
        }
    }
}
