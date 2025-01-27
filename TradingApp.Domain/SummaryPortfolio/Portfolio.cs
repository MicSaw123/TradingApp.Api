using System.ComponentModel.DataAnnotations;
using TradingApp.Domain.Base;

namespace TradingApp.Domain.SummaryPortfolio
{
    public class Portfolio : BasePortfolio
    {

        [Required]
        public int SpotPortfolioId { get; set; }

        [Required]
        public int FuturesPortfolioId { get; set; }
    }
}
