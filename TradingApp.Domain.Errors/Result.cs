using TradingApp.Domain.Errors;

namespace TradingApp.Domain.Errors
{
    public class BaseResult
    {
        public BaseResult(bool isSuccessful, Error error)
        {
            if (isSuccessful && error != Error.ErrorNone ||
                !isSuccessful && error == Error.ErrorNone)
            {
                throw new ArgumentException("Invalid error", nameof(error));
            }
            IsSuccessful = isSuccessful;
            Error = error;
        }

        public BaseResult(bool isSuccessful, Error error, params string[] errorParameters) : this(isSuccessful, error)
        {

        }

        public bool IsSuccessful { get; }

        public bool IsFailure => !IsSuccessful;

        public Error Error { get; }
    }
}

public class RequestResult : BaseResult
{
    public RequestResult(bool isSuccessful, Error error) : base(isSuccessful, error)
    {

    }

    private RequestResult(bool isSuccessful, Error error, params string[] parameters) :
        base(isSuccessful, error)
    {

    }

    public static RequestResult Success() => new(true, Error.ErrorNone);

    public static RequestResult Failure(Error error, params string[] parameters) => new(false, error, parameters);
}

public class RequestResult<T> : BaseResult
{
    public T Result { get; }

    public RequestResult(T result, bool isSuccessful, Error error) : base(isSuccessful, error)
    {
        Result = result;
    }

    private RequestResult(T result, bool isSuccessful, Error error, params string[] parameters) : base(isSuccessful, error, parameters)
    {
        Result = result;
    }

    public static RequestResult<T> Success(T result) => new RequestResult<T>(result, true, Error.ErrorNone);

    public static RequestResult<T> Failure(Error error, params string[] parameters) => new(default, false, error, parameters);

    public static RequestResult<T> Failure(RequestResult requestResult)
    {
        return new RequestResult<T>(default, requestResult.IsSuccessful, requestResult.Error);
    }
}