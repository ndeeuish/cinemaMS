namespace CinemaMS.Domain.Exceptions;

public class UserFriendlyException : BaseException
{
    public UserFriendlyException(string message, ExceptionCode code = ExceptionCode.BadRequest) 
        : base(message, code)
    {
    }
}

public class NotFoundException : BaseException
{
    public NotFoundException(string name, object key) 
        : base($"{name} id {key} not found", ExceptionCode.NotFound)
    {
    }
}

public class UnauthorizedException : BaseException
{
    public UnauthorizedException(string message) 
        : base(message, ExceptionCode.Unauthorized)
    {
    }
}
