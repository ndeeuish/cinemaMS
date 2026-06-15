using CinemaMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CinemaMS.Infrastructure.Services;

public class ExpiredBookingCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ExpiredBookingCleanupService> _logger;

    public ExpiredBookingCleanupService(IServiceProvider serviceProvider, ILogger<ExpiredBookingCleanupService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Expired Booking Cleanup Service is starting.");

        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                await CleanupExpiredBookingsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing Expired Booking Cleanup.");
            }
        }
    }

    private async Task CleanupExpiredBookingsAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var currentTime = DateTime.UtcNow;

        var rowsAffected = await dbContext.Bookings
            .Where(b => b.Status == "Holding" && b.HoldExpiration <= currentTime)
            .ExecuteUpdateAsync(s => s
                .SetProperty(b => b.Status, "Expired")
                .SetProperty(b => b.UpdatedAt, currentTime),
            cancellationToken: stoppingToken);

        if (rowsAffected > 0)
        {
            _logger.LogInformation("ExpiredBookingCleanupService: Automatically expired {Count} bookings at {Time}.", rowsAffected, currentTime);
        }
    }
}
