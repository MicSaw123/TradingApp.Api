using System.Net;
using TradingApp.Application.Services.TokenManagerService;

namespace TradingApp.Api.Middlewares
{
    public class TokenManagerMiddleware
    {
        private readonly ITokenManagerService _tokenManagerService;
        private readonly RequestDelegate _next;

        public TokenManagerMiddleware(ITokenManagerService tokenManagerService, RequestDelegate next)
        {
            _tokenManagerService = tokenManagerService;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (await _tokenManagerService.IsCurrentTokenActive())
            {
                await _next(context);
                return;
            }
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}
