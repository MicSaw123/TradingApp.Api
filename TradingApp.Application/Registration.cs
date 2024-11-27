using Microsoft.Extensions.DependencyInjection;
using TradingApp.Application.Realtime.DataQueues;
using TradingApp.Application.Repositories.CoinRepository;
using TradingApp.Application.Repositories.DbTransactionRepository;
using TradingApp.Application.Repositories.SpotPortfolioRepository;
using TradingApp.Application.Repositories.SpotTransactionRepository;
using TradingApp.Application.Repositories.SpotTransactionToAdd;
using TradingApp.Application.Services.AutoMapperProfiles;
using TradingApp.Application.Services.CoinService;
using TradingApp.Application.Services.SpotPortfolioService;
using TradingApp.Application.Services.SpotTransactionService;
using TradingApp.Application.Services.SpotTransactionToOpenService;

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
            services.AddSingleton<DataQueue>();
        }

        public static void AddServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddScoped<ICoinService, CoinService>();
            services.AddScoped<ISpotPortfolioService, SpotPortfolioService>();
            services.AddScoped<ISpotTransactionService, SpotTransactionService>();
            services.AddScoped<ISpotTransactionToOpenService, SpotTransactionToOpenService>();
        }
    }
}
