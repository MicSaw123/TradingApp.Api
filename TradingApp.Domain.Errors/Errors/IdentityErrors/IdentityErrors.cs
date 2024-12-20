using System.Net;

namespace TradingApp.Domain.Errors.Errors.IdentityErrors
{
    public static class IdentityErrors
    {
        public static readonly Error EmailAlreadyTaken = new EmailUsed();

        public static readonly Error PasswordTooShort = new PasswordShort();

        public static readonly Error PasswordRequirements = new PasswordRequirementsNotMet();

        public static readonly Error UserDoesNotExist = new NotExistingUser();

        public static readonly Error WrongPassword = new IncorrectPassword();

        public static readonly Error UsernameAlreadyTaken = new UsernameTaken();

        public static readonly Error AccountCreationError = new ErrorCreatingAccount();

        private sealed class PasswordShort : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;

            public PasswordShort() : base(nameof(PasswordTooShort), 401)
            {
                Message = "Password is too short";
            }
        }

        private sealed class ErrorCreatingAccount : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;
            public ErrorCreatingAccount() : base(nameof(AccountCreationError), 409)
            {
                Message = "There was an error while creating an account. Please try again later";
            }
        }

        private sealed class PasswordRequirementsNotMet : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;

            public PasswordRequirementsNotMet() : base(nameof(PasswordRequirements), 402)
            {
                Message = "Password doesn't meet requirements";
            }
        }

        private sealed class NotExistingUser : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;

            public NotExistingUser() : base(nameof(UserDoesNotExist), 403)
            {
                Message = "User with this email doesn't exist";
            }
        }

        private sealed class IncorrectPassword : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;

            public IncorrectPassword() : base(nameof(WrongPassword), 407)
            {
                Message = "Wrong password";
            }
        }

        private sealed class EmailUsed : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;

            public EmailUsed() : base(nameof(EmailAlreadyTaken), 408)
            {
                Message = "Account with this email already exists";
            }
        }

        private sealed class UsernameTaken : Error
        {
            public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;
            public UsernameTaken() : base(nameof(UsernameAlreadyTaken), 409)
            {
                Message = "This username is already taken";
            }
        }
    }
}