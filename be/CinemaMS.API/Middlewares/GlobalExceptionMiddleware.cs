using System.Net;
using System.Text.Json;
using CinemaMS.Application.Exceptions;
using CinemaMS.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CinemaMS.API.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception has occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = (int)HttpStatusCode.InternalServerError,
            code = (int)ExceptionCode.UnknownError,
            message = "Internal Server Error.",
            details = exception.Message,
            errors = (object?)null
        };

        switch (exception)
        {
            case ValidationException validationEx:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response = new
                {
                    status = context.Response.StatusCode,
                    code = (int)validationEx.Code,
                    message = "Validation failed",
                    details = "One or more validation failures have occurred.",
                    errors = (object?)validationEx.Errors
                };
                break;
            case UserFriendlyException userEx:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response = new
                {
                    status = context.Response.StatusCode,
                    code = (int)userEx.Code,
                    message = "Business rule violation",
                    details = userEx.Message,
                    errors = (object?)null
                };
                break;
            case NotFoundException notFoundEx:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                response = new
                {
                    status = context.Response.StatusCode,
                    code = (int)notFoundEx.Code,
                    message = "Resource not found",
                    details = notFoundEx.Message,
                    errors = (object?)null
                };
                break;
            case BaseException baseEx:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response = new
                {
                    status = context.Response.StatusCode,
                    code = (int)baseEx.Code,
                    message = "Bad request",
                    details = baseEx.Message,
                    errors = (object?)null
                };
                break;
            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
