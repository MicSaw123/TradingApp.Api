using Microsoft.Extensions.DependencyInjection;
using TradingApp.Application.Repositories.CoinRepository;
using TradingApp.Application.Repositories.DbTransactionRepository;
using TradingApp.Application.Repositories.FuturesPortfolios;
using TradingApp.Application.Repositories.FuturesTransactionsToOpen;
using TradingApp.Application.Repositories.FuturesTransactionToOpenRepository;
using TradingApp.Application.Repositories.SpotPortfolioRepository;
using TradingApp.Application.Repositories.SpotTransactionRepository;
using TradingApp.Application.Repositories.SpotTransactionsToOpen;
using TradingApp.Application.Repositories.SpotTransactionToAdd;
using TradingApp.Application.Repositories.TransactionRepository.FuturesTransactionRepository;
using TradingApp.Application.Services.AutoMapperProfiles;
using TradingApp.Application.Services.CoinService;
using TradingApp.Application.Services.ConnectionManager;
using TradingApp.Application.Services.FuturesPortfoliosService;
using TradingApp.Application.Services.FuturesTransactionsService;
using TradingApp.Application.Services.FuturesTransactionsToOpenService;
using TradingApp.Application.Services.IdentityService;
using TradingApp.Application.Services.SpotPortfolioService;
using TradingApp.Application.Services.SpotTransactionService;
using TradingApp.Application.Services.SpotTransactionToOpenService;
using TradingApp.Application.Services.TokenManagerService;

namespace TradingApp.Application
{
    public static class Registration
    {
        public static void AddApplication(this IServiceCollection services)
        {
            AddServices(services);
            AddRepositories(services);
        }

        public static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<ICoinRepository, CoinRepository>();
            services.AddScoped<ISpotPortfolioRepository, SpotPortfolioRepository>();
            services.AddScoped<ISpotTransactionRepository, SpotTransactionRepository>();
            services.AddScoped<IDbTransactionRepository, DbTransactionRepository>();
            services.AddScoped<ISpotTransactionToOpenRepository, SpotTransactionToOpenRepository>();
            services.AddScoped<IFuturesTransactionToOpenRepository, FuturesTransactionToOpenRepository>();
            services.AddScoped<IFuturesPortfolioRepository, FuturesPortfolioRepository>();
            services.AddScoped<IFuturesTransactionRepository, FuturesTransactionRepository>();
        }

        public static void AddServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSingleton<IConnectionManager, ConnectionManager>();
            services.AddSingleton<ITokenManagerService, TokenManagerService>();
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddScoped<ICoinService, CoinService>();
            services.AddScoped<ISpotPortfolioService, SpotPortfolioService>();
            services.AddScoped<ISpotTransactionService, SpotTransactionService>();
            services.AddScoped<ISpotTransactionToOpenService, SpotTransactionToOpenService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IFuturesPortfolioService, FuturesPortfolioService>();
            services.AddScoped<IFuturesTransactionsToOpenService, FuturesTransactionsToOpenService>();
            services.AddScoped<IFuturesTransactionService, FuturesTransactionService>();
        }
    }
}
