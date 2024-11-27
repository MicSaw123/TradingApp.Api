using System.Net;

namespace TradingApp.Domain.Errors.TransactionToOpenErrors
{
    public abstract class TransactionToOpenError(string Name, int StatusCode, string? Description = null)
    {
        public static readonly Error ErrorAddTransactionToOpen = new AddTransactionToOpenError();

        public static readonly Error ErrorCancelTransactionToOpen = new CancelAwaitingTransactionToOpenError();

        public static readonly Error ErrorEditTransactionToOpen = new EditTransactionToOpenError();

        public static readonly Error ErrorOpenAwaitingTransactionToOpen = new OpenAwaitingTransactionToOpenError();

        private sealed class AddTransactionToOpenError : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;
            public AddTransactionToOpenError() : base(nameof(AddTransactionToOpenError), 2001)
            {
                Message = "There was an error while creating transaction!";
            }

        }

        private sealed class EditTransactionToOpenError : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;
            public EditTransactionToOpenError() : base(nameof(EditTransactionToOpenError), 2003)
            {
                Message = "There was an error while editing awaiting transaction";
            }
        }

        private sealed class CancelAwaitingTransactionToOpenError : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;
            public CancelAwaitingTransactionToOpenError() : base(nameof(CancelAwaitingTransactionToOpenError), 2002)
            {
                Message = "There was an error while canceling awaiting transaction";
            }
        }

        private sealed class OpenAwaitingTransactionToOpenError : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;
            public OpenAwaitingTransactionToOpenError() : base(nameof(OpenAwaitingTransactionToOpenError), 2004)
            {
                Message = "There was an error while opening awaiting transaction";
            }
        }
    }
}
