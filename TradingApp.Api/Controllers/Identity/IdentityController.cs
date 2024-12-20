using Microsoft.AspNetCore.Mvc;
using TradingApp.Api.Controllers.Base;
using TradingApp.Application.DataTransferObjects.Identity;
using TradingApp.Application.Services.IdentityService;

namespace TradingApp.Api.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ApiControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            return CreateResponse(await _identityService.Login(loginDto));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            return CreateResponse(await _identityService.Register(registerDto));
        }

        [HttpPost("getUserInfo")]
        public async Task<IActionResult> GetUserInfo([FromBody] string id)
        {
            return CreateResponse(await _identityService.GetUserInfo(id));
        }
    }
}
