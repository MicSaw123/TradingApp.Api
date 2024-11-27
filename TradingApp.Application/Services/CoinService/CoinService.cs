using AutoMapper;
using System.Text.Json;
using TradingApp.Application.DataTransferObjects.Coin;
using TradingApp.Application.Repositories.CoinRepository;
using TradingApp.Application.Services.Interfaces.Database;
using TradingApp.Domain.Coins;
using TradingApp.Domain.Errors;

namespace TradingApp.Application.Services.CoinService
{
    public class CoinService : ICoinService
    {
        private readonly ICoinRepository _coinRepository;
        private readonly IDbContext _context;
        private readonly IMapper _mapper;

        public CoinService(ICoinRepository coinRepository,
            IDbContext context, IMapper mapper)
        {
            _coinRepository = coinRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<RequestResult<string>> GetCoinNameById(int coinId)
        {
            var result = await _coinRepository.GetCoinNameById(coinId);
            if (result is null)
            {
                return RequestResult<string>.Failure(Error.ErrorUnknown);
            }
            return RequestResult<string>.Success(result.Result);
        }

        public async Task<RequestResult<IEnumerable<CoinDto>>> GetCoins()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<CoinDto> coinList = new List<CoinDto>();
            var coins = await _coinRepository.GetAllCoins();
            if (coins.Result is not null)
            {
                foreach (var coin in coins.Result)
                {
                    if (coin.Symbol.EndsWith("USDT"))
                    {
                        coinList.Add(coin);
                    }
                }
            }
            var cl = coinList.AsEnumerable().OrderBy(c => c.Symbol);
            return RequestResult<IEnumerable<CoinDto>>.Success(cl);
        }



        public async Task<RequestResult<IEnumerable<CoinDto>>> GetCoinsBySymbol(List<string> symbols)
        {
            var coins = await _coinRepository.GetCoinsBySymbol(symbols);
            return RequestResult<IEnumerable<CoinDto>>.Success(coins.Result);
        }

        public async Task<RequestResult<IEnumerable<CoinDto>>> GetCoinsPerPage(int pageSize,
            int page)
        {
            var coinPage = await GetCoins();
            var x = coinPage.Result.Skip((page - 1) * pageSize).Take(pageSize);
            if (x is not null)
            {
                return RequestResult<IEnumerable<CoinDto>>.Success(x);
            }
            return RequestResult<IEnumerable<CoinDto>>.Failure(Error.ErrorUnknown);
        }

        public async Task<RequestResult> SeedCoins(CancellationToken cancellation = default)
        {
            var result = await GetCoins();
            var coins = _mapper.Map<List<Coin>>(result.Result);
            if (coins != null)
            {
                await _context.Set<Coin>().AddRangeAsync(coins, cancellation);
                await _context.SaveChangesAsync(cancellation);
                return RequestResult.Success();
            }
            return RequestResult.Failure(Error.ErrorUnknown);
        }
    }
}
