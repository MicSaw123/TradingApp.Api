using AutoMapper;
using TradingApp.Application.DataTransferObjects.Coin;
using TradingApp.Application.DataTransferObjects.Portfolio;
using TradingApp.Application.DataTransferObjects.Spot;
using TradingApp.Application.DataTransferObjects.Transaction;
using TradingApp.Domain.Coins;
using TradingApp.Domain.Futures;
using TradingApp.Domain.Portfolio;
using TradingApp.Domain.Spot;

namespace TradingApp.Application.Services.AutoMapperProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SpotTransactionDto, SpotTransaction>().ReverseMap();
            CreateMap<FuturesTransactionDto, FuturesTransaction>().ReverseMap();
            CreateMap<SpotPortfolioDto, SpotPortfolio>().ReverseMap();
            CreateMap<PortfolioDto, Portfolio>().ReverseMap();
            CreateMap<CoinDto, Coin>().ReverseMap();
            CreateMap<SpotTransactionToOpenDto, SpotTransactionToOpen>().ReverseMap();
            CreateMap<SpotTransactionToOpen, SpotTransaction>()
                .ForMember(x => x.Id, opt => opt.Ignore()).ReverseMap();
        }
    }
}
