namespace TradingApp.Application.Services.TokenManagerService
{
    public interface ITokenManagerService
    {
        Task<bool> IsCurrentTokenActive();

        Task DeactivateCurrentTokenAsync();

        Task<bool> IsActiveAsync(string token);

        Task DeactivateToken(string token);
    }
}
