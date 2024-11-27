using System.Net;

namespace TradingApp.Domain.Errors.Errors.SpotPortfolioErrors
{
    public abstract class PortfolioError
    {
        public static readonly Error ErrorAddFunds = new PortfolioAddFundsError();

        public static readonly Error NonSufficientFunds = new NonSufficientBalanceError();

        public static readonly Error ErrorGetPortfolioByUserId = new GetPortfolioByUserIdError();

        public static readonly Error ErrorGetPortfolioById = new GetPortfolioByIdError();

        public static readonly Error ErrorCalculatePortfolioProfits = new CalculateProfitsError();

        private sealed class PortfolioAddFundsError : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;
            public PortfolioAddFundsError() : base(nameof(PortfolioAddFundsError), 3001)
            {
                Message = "There was an error while adding funds to this portfolio";
            }
        }

        private sealed class NonSufficientBalanceError : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;
            public NonSufficientBalanceError() : base(nameof(NonSufficientBalanceError), 3002)
            {
                Message = "Non sufficient balance to proceed with transaction";
            }
        }

        private sealed class GetPortfolioByUserIdError : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;
            public GetPortfolioByUserIdError() : base(nameof(GetPortfolioByUserIdError), 3003)
            {
                Message = "Portfolio with this user Id doesn't exist";
            }
        }

        private sealed class GetPortfolioByIdError : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;
            public GetPortfolioByIdError() : base(nameof(ErrorGetPortfolioById), 3004)
            {
                Message = "Portfolio with this Id doesn't exist";
            }
        }

        private sealed class CalculateProfitsError : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;
            public CalculateProfitsError() : base(nameof(ErrorCalculatePortfolioProfits), 3005)
            {
                Message = "There was an error while calculating portfolio's profits";
            }
        }
    }
}
