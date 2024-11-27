using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TradingApp.Application.Services.Interfaces.Database;
using TradingApp.Database.Context;

namespace TradingApp.Database
{
    public static class Registration
    {
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TradingAppContext>
                (options => options.UseSqlServer(configuration.GetConnectionString("TradingApp")));
            services.AddScoped<IDbContext, TradingAppContext>();
        }
    }
}
