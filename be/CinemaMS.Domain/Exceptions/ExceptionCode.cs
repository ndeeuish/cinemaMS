namespace CinemaMS.Domain.Exceptions;

public enum ExceptionCode
{
    Success = 0,
    UnknownError = 1,
    ValidationError = 2,
    NotFound = 3,
    BadRequest = 4,
    Unauthorized = 5,
    Forbidden = 6
}
