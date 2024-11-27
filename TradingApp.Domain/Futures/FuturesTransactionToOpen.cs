using System.Diagnostics.CodeAnalysis;
using TradingApp.Domain.Base;

namespace TradingApp.Domain.Futures
{
    public class FuturesTransactionToOpen : BaseTransactionToOpen
    {
        public int Leverage { get; set; }

        public float TotalTransactionWorth { get; set; }

        [AllowNull]
        public float ClosingPrice { get; set; }

        [AllowNull]
        public float TakeProfitPrice { get; set; }

        public int FuturesPortfolioId { get; set; }

        public FuturesPortfolio FuturesPortfolio { get; set; } = new FuturesPortfolio();
    }
}
