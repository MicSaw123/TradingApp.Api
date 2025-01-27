using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TradingApp.Application.DataTransferObjects.Identity;
using TradingApp.Application.Services.FuturesPortfoliosService;
using TradingApp.Application.Services.Interfaces.Database;
using TradingApp.Application.Services.PortfolioService;
using TradingApp.Application.Services.SpotPortfolioService;
using TradingApp.Database.TradingAppUsers;
using TradingApp.Domain.Errors;
using TradingApp.Domain.Errors.Errors.IdentityErrors;
using TradingApp.Domain.Futures;
using TradingApp.Domain.Spot;
using TradingApp.Domain.SummaryPortfolio;

namespace TradingApp.Application.Services.IdentityService
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<TradingAppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISpotPortfolioService _spotPortfolioService;
        private readonly IFuturesPortfolioService _futuresPortfolioService;
        private readonly IPortfolioService _portfolioService;

        public IdentityService(UserManager<TradingAppUser> userManager,
            IConfiguration configuration, IDbContext context, IMapper mapper, ISpotPortfolioService spotPortfolioService,
            IFuturesPortfolioService futuresPortfolioService, IPortfolioService portfolioService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
            _spotPortfolioService = spotPortfolioService;
            _futuresPortfolioService = futuresPortfolioService;
            _portfolioService = portfolioService;
        }

        public async Task<RequestResult<UserInfoDto>> GetUserInfo(string id)
        {
            var user = await _context.Set<TradingAppUser>().FirstOrDefaultAsync(x => x.Id == id);
            var userDto = _mapper.Map<UserInfoDto>(user);
            if (user is null)
            {
                return RequestResult<UserInfoDto>.Failure(Error.ErrorUnknown);
            }
            return RequestResult<UserInfoDto>.Success(userDto);
        }

        public async Task<RequestResult<LoginResponseDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null)
            {
                return RequestResult<LoginResponseDto>.Failure(IdentityErrors.UserDoesNotExist);
            }
            var passwordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!passwordCorrect)
            {
                return RequestResult<LoginResponseDto>.Failure(IdentityErrors.WrongPassword);
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            var token = GetToken(authClaims);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            var loginResponse = new LoginResponseDto
            {
                Token = tokenString
            };
            return RequestResult<LoginResponseDto>.Success(loginResponse);
        }

        public async Task<RequestResult> Register(RegisterDto registerDto)
        {
            CancellationToken cancellation = default;
            var emailExists = await _userManager.FindByEmailAsync(registerDto.Email);
            if (emailExists != null)
            {
                return IdentityErrors.EmailAlreadyTaken;
            }
            TradingAppUser user = new()
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                PhoneNumber = registerDto.PhoneNumber,
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return IdentityErrors.AccountCreationError;
            }
            FuturesPortfolio futuresPortfolio = new FuturesPortfolio
            {
                UserId = user.Id,
                AllTransactionsWorth = 0,
                Balance = 0,
                DailyProfit = 0,
                MonthlyProfit = 0,
                WeeklyProfit = 0
            };
            SpotPortfolio spotPortfolio = new SpotPortfolio
            {
                UserId = user.Id,
                AllTransactionsWorth = 0,
                Balance = 0,
                DailyProfit = 0,
                MonthlyProfit = 0,
                WeeklyProfit = 0
            };
            Portfolio portfolio = new Portfolio
            {
                UserId = user.Id,
                AllTransactionsWorth = 0,
                Balance = 0,
                DailyProfit = 0,
                MonthlyProfit = 0,
                WeeklyProfit = 0,
                FuturesPortfolioId = futuresPortfolio.Id,
                SpotPortfolioId = spotPortfolio.Id
            };
            await _futuresPortfolioService.AddFuturesPortfolio(futuresPortfolio, cancellation);
            await _spotPortfolioService.AddSpotPortfolio(spotPortfolio, cancellation);
            await _portfolioService.AddPortfolio(portfolio, cancellation);
            return RequestResult.Success();
        }


        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                expires: DateTime.UtcNow.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
