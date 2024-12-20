using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using TradingApp.Application.DataTransferObjects.Coin;
using TradingApp.Application.DataTransferObjects.PaginationDto;
using TradingApp.Application.Services.ConnectionManager;

namespace TradingApp.Application.Realtime
{
    public class CoinListHub : Hub<ICoinListHub>
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IConnectionManager _connectionManager;

        public CoinListHub(IMemoryCache memoryCache, IConnectionManager connectionManager)
        {

            _memoryCache = memoryCache;
            _connectionManager = connectionManager;
        }

        public async Task GetPaginationParameters(PaginationDto paginationDto)
        {
            var connectionList = _connectionManager.GetAllConnections().Result;
            foreach (var connection in connectionList)
            {
                var userId = _memoryCache.Get(connection);
                if (userId is null)
                {
                    continue;
                }
                _memoryCache.Set(userId, paginationDto);
            }
        }

        public async Task GetCoinsPerPage(List<CoinDto> coins)
        {
            return;
        }

        public override async Task OnConnectedAsync()
        {
            _connectionManager.AddConnectionIdToList(Context.ConnectionId);
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.Request.Query["userId"];
            _memoryCache.Set(Context.ConnectionId, userId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _memoryCache.Remove(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
