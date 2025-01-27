using System.Net;

namespace TradingApp.Domain.Errors.Errors.TransactionErrors
{
    public class TransactionError
    {
        public static readonly Error ErrorGetTransactionsByPortfolioId = new GetTransactionsByPortfolioIdError();

        public static readonly Error ErrorCloseTransaction = new CloseTransactionError();

        public static readonly Error ErrorCalculateTransactionProfits = new CalculateTransactionProfitsError();

        public static readonly Error ErrorEditTransaction = new EditTransactionError();

        public static readonly Error ErrorLiquidateFuturesTransactions = new LiquidateFuturesTransactionsError();

        public static readonly Error ErrorRemoveTransactionProfit = new RemoveTransactionProfitError();

        private class GetTransactionsByPortfolioIdError : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;

            public GetTransactionsByPortfolioIdError() : base(nameof(ErrorGetTransactionsByPortfolioId), 901)
            {
                Message = "There was an error while fetching transactions for this portfolio";
            }
        }

        private class CloseTransactionError : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;
            public CloseTransactionError() : base(nameof(ErrorCloseTransaction), 902)
            {
                Message = "There was an error while closing transaction! Try again later.";
            }
        }

        private class CalculateTransactionProfitsError : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;

            public CalculateTransactionProfitsError() : base(nameof(ErrorCalculateTransactionProfits), 903)
            {
                Message = "There was an error while calculating transcation profits";
            }
        }

        private class EditTransactionError : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;

            public EditTransactionError() : base(nameof(ErrorEditTransaction), 904)
            {
                Message = "There was an error while editing transaction. Try again later.";
            }
        }

        private class LiquidateFuturesTransactionsError : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;

            public LiquidateFuturesTransactionsError() : base(nameof(ErrorLiquidateFuturesTransactions), 905)
            {
                Message = "There was an error while liquidating transactions for this portfolio";
            }
        }

        private class RemoveTransactionProfitError : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;
            public RemoveTransactionProfitError() : base(nameof(ErrorRemoveTransactionProfit), 906)
            {
                Message = "There was an error while removing profit from portfolio!";
            }
        }
    }
}
