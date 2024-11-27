using System.ComponentModel.DataAnnotations;
using TradingApp.Database.TradingAppUsers;
using TradingApp.Domain.Base;
using TradingApp.Domain.Futures;
using TradingApp.Domain.Spot;

namespace TradingApp.Domain.Portfolio
{
    public class Portfolio : BasePortfolio
    {
        public TradingAppUser TradingAppUser { get; set; }

        [Required]
        public string TradingAppUserId { get; set; }

        public SpotPortfolio SpotPortfolio { get; set; }

        [Required]
        public int SpotPortfolioId { get; set; }

        public FuturesPortfolio FuturesPortfolio { get; set; }

        [Required]
        public int FuturesPortfolioId { get; set; }
    }
}
