using AutoMapper;
using System.Net.Http.Json;
using System.Text.Json;
using TradingApp.Application.DataTransferObjects.Coin;
using TradingApp.Application.DataTransferObjects.PaginationDto;
using TradingApp.Application.Repositories.Coins;
using TradingApp.Application.Services.Interfaces.Database;
using TradingApp.Domain.Coins;
using TradingApp.Domain.Errors.Errors.CoinErrors;

namespace TradingApp.Application.Services.CoinService
{
    public class CoinService : ICoinService
    {
        private readonly IDbContext _context;
        private readonly IMapper _mapper;
        private readonly HttpClient _http;
        private readonly ICoinRepository _coinRepository;
        private readonly string baseApiAddress = "https://api.binance.com/api/v3/ticker/price";

        public CoinService(IDbContext context, IMapper mapper, HttpClient http, ICoinRepository coinRepository)
        {
            _context = context;
            _mapper = mapper;
            _http = http;
            _coinRepository = coinRepository;
        }

        public async Task<IEnumerable<CoinDto>> GetAllCoins()
        {
            var result = await _http.GetFromJsonAsync<IEnumerable<CoinDto>>(baseApiAddress);
            return result;
        }

        public async Task<RequestResult<IEnumerable<CoinDto>>> GetCoins()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<CoinDto> coinList = new List<CoinDto>();
            var coins = await GetAllCoins();
            if (coins is not null)
            {
                foreach (var coin in coins)
                {
                    if (coin.Symbol.EndsWith("USDT") && coin.Price > 0)
                    {
                        coinList.Add(coin);
                    }
                }
            }
            else
            {
                return RequestResult<IEnumerable<CoinDto>>.Failure(CoinError.ErrorFetchCoins);
            }
            var cl = coinList.AsEnumerable().OrderBy(c => c.Symbol);
            return RequestResult<IEnumerable<CoinDto>>.Success(cl);
        }

        public async Task<RequestResult<CoinDto>> GetCoinBySymbol(string symbol)
        {
            var coin = await _http.GetFromJsonAsync<CoinDto>(baseApiAddress + $"?symbol={symbol}");
            if (coin is null)
            {
                return RequestResult<CoinDto>.Failure(CoinError.ErrorFetchCoins);
            }
            return RequestResult<CoinDto>.Success(coin);
        }

        public async Task<RequestResult<IEnumerable<CoinDto>>> GetCoinsBySymbol(List<string> symbols)
        {
            List<CoinDto> coinList = new List<CoinDto>();
            foreach (var symbol in symbols)
            {
                var coin = await GetCoinBySymbol(symbol);
                if (coin is null)
                {
                    continue;
                }
                coinList.Add(coin.Result);
            }
            if (coinList is null)
            {
                return RequestResult<IEnumerable<CoinDto>>.Failure(CoinError.ErrorFetchCoins);
            }
            return RequestResult<IEnumerable<CoinDto>>.Success(coinList.AsEnumerable());
        }

        public async Task<RequestResult<IEnumerable<CoinDto>>> GetCoinsPerPage(PaginationDto paginationDto)
        {
            var coinPage = await GetCoins();
            var x = coinPage.Result.Skip((paginationDto.Page - 1) * paginationDto.PageSize).Take(paginationDto.PageSize);
            if (x is not null)
            {
                return RequestResult<IEnumerable<CoinDto>>.Success(x);
            }
            return RequestResult<IEnumerable<CoinDto>>.Failure(CoinError.ErrorGetCoinsPerPage);
        }

        public async Task<RequestResult> SeedCoins(CancellationToken cancellation = default)
        {
            var result = await GetCoins();
            List<Coin> coins = new List<Coin>();
            foreach (var coin in result.Result)
            {
                var mappedCoin = _mapper.Map<Coin>(coin);
                mappedCoin.AllTimeHighPrice = coin.Price;
                mappedCoin.AllTimeLowPrice = coin.Price;
                coins.Add(mappedCoin);
            }
            if (coins != null)
            {
                await _coinRepository.AddCoins(coins, cancellation);
                return RequestResult.Success();
            }
            return RequestResult.Failure(CoinError.ErrorSeedCoins);
        }

        public async Task<RequestResult> UpdateAllTimeValues(CancellationToken cancellation)
        {
            var storedCoins = await _coinRepository.GetCoins();
            foreach (var coin in storedCoins)
            {
                var currentValues = await GetCoinBySymbol(coin.Symbol);
                if (currentValues.Result.Price > coin.AllTimeHighPrice)
                {
                    coin.AllTimeHighPrice = currentValues.Result.Price;
                }
                if (currentValues.Result.Price < coin.AllTimeLowPrice)
                {
                    coin.AllTimeLowPrice = currentValues.Result.Price;
                }
            }
            await _coinRepository.EditCoins(storedCoins, cancellation);
            if (cancellation.IsCancellationRequested)
            {
                return RequestResult.Failure(CoinError.ErrorUpdateCoins);
            }
            return RequestResult.Success();
        }
    }
}
