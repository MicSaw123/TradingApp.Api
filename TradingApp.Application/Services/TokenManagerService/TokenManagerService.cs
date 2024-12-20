using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Primitives;

namespace TradingApp.Application.Services.TokenManagerService
{
    public class TokenManagerService : ITokenManagerService
    {
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenManagerService(IDistributedCache cache, IHttpContextAccessor httpContextAccessor)
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> IsCurrentTokenActive()
            => await IsActiveAsync(GetCurrentAsync());

        public async Task DeactivateCurrentTokenAsync()
            => await DeactivateToken(GetCurrentAsync());

        public async Task<bool> IsActiveAsync(string token)
            => await _cache.GetStringAsync(GetKey(token)) == null;


        public async Task DeactivateToken(string token)
            => await _cache.SetStringAsync(GetKey(token),
            " ", new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow =
                TimeSpan.FromMinutes(10)
            });


        private string GetCurrentAsync()
        {
            var authorizationHeader = _httpContextAccessor
                .HttpContext.Request.Headers["Authorization"];

            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(" ").Last();
        }

        private static string GetKey(string token)
            => $"tokens:{token}:deactivated";
    }
}
