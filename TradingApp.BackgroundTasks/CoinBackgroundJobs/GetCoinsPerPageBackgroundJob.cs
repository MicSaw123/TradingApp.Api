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
    public class GetCoinsPerPageBackgroundJob : BackgroundService
    {
        private readonly IHubContext<CoinListHub, ICoinListHub> _hubContext;
        private readonly IMemoryCache _memoryCache;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnectionManager _connectionManager;

        public GetCoinsPerPageBackgroundJob(IHubContext<CoinListHub, ICoinListHub> hubContext,
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
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var connectionList = (await _connectionManager.GetAllConnections()).ToList();
                    if (connectionList.Count > 0)
                    {
                        foreach (var connection in connectionList)
                        {
                            var userId = _memoryCache.Get(connection);
                            if (userId is null)
                            {
                                continue;
                            }
                            var pageInfo = (PaginationDto)_memoryCache.Get(userId);
                            if (pageInfo is null)
                            {
                                pageInfo = new PaginationDto()
                                {
                                    Page = 1,
                                    PageSize = 15
                                };
                                _memoryCache.Set(userId, pageInfo);
                            }
                            using (var scope = _serviceProvider.CreateScope())
                            {
                                var coinService = scope.ServiceProvider.GetService<ICoinService>();
                                var coins = await coinService!.GetCoinsPerPage(pageInfo);
                                var coinList = coins.Result.ToList();
                                await _hubContext.Clients.Client(connection).GetCoinsPerPage(coinList);
                            }
                        }
                    }
                    await Task.Delay(5000);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
