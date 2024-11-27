using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TradingApp.Application.Services.Interfaces.Database;
using TradingApp.Database.TradingAppUsers;
using TradingApp.Domain.Coins;
using TradingApp.Domain.Futures;
using TradingApp.Domain.Portfolio;
using TradingApp.Domain.Spot;

namespace TradingApp.Database.Context
{
    public class TradingAppContext : IdentityDbContext<TradingAppUser>, IDbContext
    {
        public TradingAppContext(DbContextOptions<TradingAppContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Coin> Coins { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }

        public DbSet<SpotPortfolio> SpotPortfolios { get; set; }

        public DbSet<FuturesPortfolio> FuturesPortfolios { get; set; }

        public DbSet<SpotTransaction> SpotTransactions { get; set; }

        public DbSet<SpotTransactionToOpen> SpotTransactionsToOpen { get; set; }

        public DbSet<FuturesTransaction> FuturesTransactions { get; set; }

        public DbSet<FuturesTransactionToOpen> FuturesTransactionsToOpen { get; set; }
    }
}
