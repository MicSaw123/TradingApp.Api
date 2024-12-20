using System.Diagnostics.CodeAnalysis;

namespace TradingApp.Application.DataTransferObjects.Transaction
{
    public class FuturesTransactionDto : TransactionDto
    {
        public int FuturesPortfolioId { get; set; }

        public int Leverage { get; set; }

        [AllowNull]
        public float ClosingPrice { get; set; }
    }
}
