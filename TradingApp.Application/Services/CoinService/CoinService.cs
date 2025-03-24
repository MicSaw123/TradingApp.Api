using AutoMapper;
using System.Net.Http.Json;
using System.Text.Json;
using TradingApp.Application.DataTransferObjects.Coin;
using TradingApp.Application.DataTransferObjects.PaginationDto;
using TradingApp.Application.Repositories.Coins;
using TradingApp.Domain.Coins;
using TradingApp.Domain.Errors.Errors.CoinErrors;

namespace TradingApp.Application.Services.CoinService
{
    public class CoinService : ICoinService
    {
        private readonly IMapper _mapper;
        private readonly HttpClient _http;
        private readonly ICoinRepository _coinRepository;
        private readonly string baseApiAddress = "https://api.binance.com/api/v3/ticker/price";

        public CoinService(IMapper mapper, HttpClient http, ICoinRepository coinRepository)
        {
            _mapper = mapper;
            _http = http;
            _coinRepository = coinRepository;
        }

        public async Task<RequestResult<List<CoinDto>>> GetCoins()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<CoinDto> coinList = new List<CoinDto>();
            var result = await _http.GetFromJsonAsync<List<CoinDto>>(baseApiAddress);
            if (result is not null)
            {
                foreach (var coin in result)
                {
                    if (coin.Symbol.EndsWith("USDT") && coin.Price > 0)
                    {
                        var dbCoin = await _coinRepository.GetCoinBySymbol(coin.Symbol);
                        if (dbCoin is null)
                        {
                            continue;
                        }
                        coin.AllTimeHighPrice = dbCoin.AllTimeHighPrice;
                        coin.AllTimeLowPrice = dbCoin.AllTimeLowPrice;
                        coinList.Add(coin);
                    }
                }
            }
            var orderedCoinList = coinList.OrderBy(x => x.Symbol).ToList();
            return RequestResult<List<CoinDto>>.Success(orderedCoinList);
        }

        public async Task<RequestResult<CoinDto>> GetCoinBySymbol(string symbol)
        {
            var coin = await _http.GetFromJsonAsync<CoinDto>(baseApiAddress + $"?symbol={symbol}");
            if (coin is null)
            {
                return RequestResult<CoinDto>.Failure(CoinError.ErrorFetchCoins);
            }
            var allTimeValues = await _coinRepository.GetCoinBySymbol(symbol);
            coin.AllTimeHighPrice = allTimeValues.AllTimeHighPrice;
            coin.AllTimeLowPrice = allTimeValues.AllTimeLowPrice;
            return RequestResult<CoinDto>.Success(coin);
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
            var fetchedCoins = await _http.GetFromJsonAsync<List<CoinDto>>(baseApiAddress);
            List<Coin> coins = new List<Coin>();
            foreach (var coin in fetchedCoins)
            {
                if (coin.Symbol.EndsWith("USDT") && coin.Price > 0)
                {
                    var mappedCoin = _mapper.Map<Coin>(coin);
                    mappedCoin.AllTimeHighPrice = coin.Price;
                    mappedCoin.AllTimeLowPrice = coin.Price;
                    coins.Add(mappedCoin);
                }
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
            try
            {
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
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
