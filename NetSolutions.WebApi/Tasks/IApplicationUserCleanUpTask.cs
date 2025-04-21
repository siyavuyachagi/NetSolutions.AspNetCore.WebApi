using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetSolutions.WebApi.Data;

namespace NetSolutions.WebApi.Tasks;

public class ApplicationUserCleanUpTask : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ApplicationUserCleanUpTask> _logger;

    public ApplicationUserCleanUpTask(IServiceScopeFactory scopeFactory, ILogger<ApplicationUserCleanUpTask> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.Now;
        var midnight = now.Date.AddDays(1); // Next day's 00:00
        var initialDelay = midnight - now;

        //_timer = new Timer(async _ => await DoCleanupAsync(), null, initialDelay, TimeSpan.FromDays(1));
        _timer = new Timer(async _ => await DoCleanupAsync(), null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
        _logger.LogInformation("User cleanup task scheduled to start at {Midnight} and repeat every 24 hours.", midnight);

        return Task.CompletedTask;
    }


    private async Task DoCleanupAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try
        {
            var thresholdDate = DateTime.UtcNow.AddDays(-7);

            var usersToDelete = await context.Users
                .Where(u => u.IsDeleted && u.UpdatedAt <= thresholdDate)
                .ToListAsync();

            if (usersToDelete.Any())
            {
                context.Users.RemoveRange(usersToDelete);
                await context.SaveChangesAsync();
                _logger.LogInformation("Deleted {Count} users during cleanup at {Time}.", usersToDelete.Count, DateTime.UtcNow);
            }
            else
            {
                _logger.LogInformation("No users to clean up at {Time}.", DateTime.UtcNow);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during user cleanup.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("User cleanup task is stopping.");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
