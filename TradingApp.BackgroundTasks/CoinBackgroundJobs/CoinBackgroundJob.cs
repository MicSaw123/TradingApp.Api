using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TradingApp.Application.DataTransferObjects.PaginationDto;
using TradingApp.Application.Realtime;
using TradingApp.Application.Services.CoinService;
using TradingApp.Application.Services.ConnectionManager;

namespace TradingApp.BackgroundTasks.CoinBackgroundJobs
{
    public class CoinBackgroundJob : BackgroundService
    {
        private readonly IHubContext<CoinListHub, ICoinListHub> _hubContext;
        private readonly IMemoryCache _memoryCache;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnectionManager _connectionManager;

        public CoinBackgroundJob(IHubContext<CoinListHub, ICoinListHub> hubContext,
            IMemoryCache memoryCache, IServiceProvider serviceProvider, IConnectionManager connectionManager)
        {
            _hubContext = hubContext;
            _memoryCache = memoryCache;
            _serviceProvider = serviceProvider;
            _connectionManager = connectionManager;
        }

        public override async Task StopAsync(CancellationToken cancellation)
        {
            await base.StopAsync(cancellation);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var connectionList = await _connectionManager.GetAllConnections();
                foreach (var connection in connectionList)
                {
                    var userId = _memoryCache.Get(connection);
                    var pageInfo = (PaginationDto)_memoryCache.Get(userId);
                    if (pageInfo is null)
                    {
                        continue;
                    }
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var coinService = scope.ServiceProvider.GetService<ICoinService>();
                        var coins = await coinService!.GetCoinsPerPage(pageInfo);
                        var coinList = coins.Result.ToList();
                        await _hubContext.Clients.Client(connection).GetCoinsPerPage(coinList);
                    }
                }
                await Task.Delay(5000);
            }
        }
    }
}
