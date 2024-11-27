using TradingApp.Domain.Base;

namespace TradingApp.Domain.Spot
{
    public class SpotTransactionToOpen : BaseTransactionToOpen
    {
        public int SpotPortfolioId { get; set; }

    }
}
