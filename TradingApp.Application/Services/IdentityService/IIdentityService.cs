using TradingApp.Application.DataTransferObjects.Identity;

namespace TradingApp.Application.Services.IdentityService
{
    public interface IIdentityService
    {
        Task<RequestResult> Register(RegisterDto registerDto);

        Task<RequestResult<LoginResponseDto>> Login(LoginDto loginDto);

        Task<RequestResult<UserInfoDto>> GetUserInfo(string id);

    }
}
