using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CinemaMS.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("Processing request {RequestName}", requestName);
        var timer = new Stopwatch();
        timer.Start();

        try
        {
            var response = await next();
            timer.Stop();
            
            if (timer.ElapsedMilliseconds > 500)
            {
                _logger.LogWarning("Long running request: {RequestName} ({ElapsedMilliseconds} milliseconds)", requestName, timer.ElapsedMilliseconds);
            }
            
            _logger.LogInformation("Completed request {RequestName} successfully", requestName);
            return response;
        }
        catch (Exception ex)
        {
            timer.Stop();
            _logger.LogError(ex, "Request {RequestName} failed after {ElapsedMilliseconds} milliseconds", requestName, timer.ElapsedMilliseconds);
            throw;
        }
    }
}
