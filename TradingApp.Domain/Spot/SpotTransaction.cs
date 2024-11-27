using TradingApp.Domain.Base;

namespace TradingApp.Domain.Spot
{
    public class SpotTransaction : BaseTransaction
    {
        public int SpotPortfolioId { get; set; }
    }
}
