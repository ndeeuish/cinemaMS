namespace CinemaMS.Domain.Exceptions;

public abstract class BaseException : Exception
{
    public ExceptionCode Code { get; }

    protected BaseException(string message, ExceptionCode code = ExceptionCode.UnknownError) 
        : base(message)
    {
        Code = code;
    }
}
