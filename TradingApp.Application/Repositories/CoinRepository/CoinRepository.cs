using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using TradingApp.Application.DataTransferObjects.Coin;
using TradingApp.Application.Services.Interfaces.Database;
using TradingApp.Domain.Coins;
using TradingApp.Domain.Errors;

namespace TradingApp.Application.Repositories.CoinRepository
{
    public class CoinRepository : ICoinRepository
    {
        private readonly IDbContext _context;
        private readonly HttpClient _http;
        private readonly IMapper _mapper;
        private string baseApiAddress = "https://api.binance.com/api/v3/ticker/price";

        public CoinRepository(IDbContext context,
            HttpClient http, IMapper mapper)
        {
            _context = context;
            _http = http;
            _mapper = mapper;
        }

        public async Task<RequestResult<IEnumerable<CoinDto>>> GetAllCoins()
        {
            var result = await _http.GetFromJsonAsync<IEnumerable<CoinDto>>(baseApiAddress);
            if (result is null)
            {
                return RequestResult<IEnumerable<CoinDto>>.Failure(Error.ErrorUnknown);
            }
            return RequestResult<IEnumerable<CoinDto>>.Success(result);
        }

        public async Task<RequestResult<string>> GetCoinNameById(int coinId)
        {
            var coin = await _context.Set<Coin>().FirstAsync(c => c.Id == coinId);
            string coinName = coin.Symbol;
            if (coinName is null)
            {
                return RequestResult<string>.Failure(Error.ErrorUnknown);
            }
            return RequestResult<string>.Success(coinName);
        }

        public async Task<RequestResult<IEnumerable<CoinDto>>> GetCoinsBySymbol(
            List<string> symbols)
        {
            List<CoinDto> coinList = new List<CoinDto>();
            foreach (var symbol in symbols)
            {
                var coin = await GetCoinBySymbol(symbol);
                if (coin.Result is null)
                {
                    continue;
                }
                coinList.Add(coin.Result);
            }
            coinList.AsEnumerable();
            return RequestResult<IEnumerable<CoinDto>>.Success(coinList);
        }

        public async Task<RequestResult<CoinDto>> GetCoinBySymbol(string coinSymbol)
        {
            var coin = await _http.GetFromJsonAsync<CoinDto>(baseApiAddress + $"?symbol={coinSymbol}");
            if (coin is null)
            {
                return RequestResult<CoinDto>.Failure(Error.ErrorUnknown);
            }
            return RequestResult<CoinDto>.Success(coin);
        }

        public async Task<RequestResult<IEnumerable<CoinDto>>> GetCoinsPerPage(int pageSize,
            int page)
        {
            var coinList = new List<CoinDto>();
            var coins = _context.Set<Coin>().Take(pageSize).Skip((page - 1) * pageSize)
                .Select(x => x.Symbol).ToList();
            var coinsBySymbol = await GetCoinsBySymbol(coins);
            var result = _mapper.Map<IEnumerable<CoinDto>>(coinsBySymbol.Result);
            if (result is null)
            {
                return RequestResult<IEnumerable<CoinDto>>.Failure(Error.ErrorUnknown);
            }
            return RequestResult<IEnumerable<CoinDto>>.Success(result);
        }
    }
}
