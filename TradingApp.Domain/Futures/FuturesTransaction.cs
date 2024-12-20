using TradingApp.Domain.Base;

namespace TradingApp.Domain.Futures
{
    public class FuturesTransaction : BaseTransaction
    {
        public int Leverage { get; set; }

        public float ClosingPrice { get; set; }

        public float LiquidationPrice { get; set; }

        public int FuturesPortfolioId { get; set; }
    }
}
