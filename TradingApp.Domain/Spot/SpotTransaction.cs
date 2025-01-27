using TradingApp.Domain.Base;

namespace TradingApp.Domain.Spot
{
    public class SpotTransaction : BaseTransaction
    {
        public float CurrentValue { get; set; }

        public int SpotPortfolioId { get; set; }

        public float SellingPrice { get; set; }
    }
}
