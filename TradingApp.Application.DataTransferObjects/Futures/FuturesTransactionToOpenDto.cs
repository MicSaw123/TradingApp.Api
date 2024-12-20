using System.Diagnostics.CodeAnalysis;

namespace TradingApp.Application.DataTransferObjects.Futures
{
    public class FuturesTransactionToOpenDto
    {
        public int Id { get; set; }

        public float BuyingPrice { get; set; }

        public float MoneyInput { get; set; }

        public string CoinSymbol { get; set; } = string.Empty;

        public int Leverage { get; set; }

        [AllowNull]
        public float ClosingPrice { get; set; }

        [AllowNull]
        public float TakeProfitPrice { get; set; }

        public int FuturesPortfolioId { get; set; }
    }
}
