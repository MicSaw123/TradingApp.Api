using AutoMapper;
using TradingApp.Application.DataTransferObjects.Coin;
using TradingApp.Application.DataTransferObjects.Futures;
using TradingApp.Application.DataTransferObjects.Identity;
using TradingApp.Application.DataTransferObjects.Portfolio;
using TradingApp.Application.DataTransferObjects.Spot;
using TradingApp.Application.DataTransferObjects.Transaction;
using TradingApp.Database.TradingAppUsers;
using TradingApp.Domain.Coins;
using TradingApp.Domain.Futures;
using TradingApp.Domain.Spot;
using TradingApp.Domain.SummaryPortfolio;

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
            CreateMap<FuturesTransactionToOpenDto, FuturesTransactionToOpen>().ReverseMap();
            CreateMap<SpotTransactionToOpenDto, SpotTransactionToOpen>().ReverseMap();
            CreateMap<SpotTransactionToOpen, SpotTransaction>()
                .ForMember(x => x.Id, opt => opt.Ignore()).ReverseMap();
            CreateMap<FuturesTransactionToOpenDto, FuturesTransaction>()
                .ForMember(x => x.Id, opt => opt.Ignore()).ReverseMap();
            CreateMap<UserInfoDto, TradingAppUser>().ReverseMap();
        }
    }
}
