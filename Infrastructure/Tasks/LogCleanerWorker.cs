using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Tasks
{
    public class LogCleanerWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<LogCleanerWorker> _logger;

        public LogCleanerWorker(IServiceProvider serviceProvider, ILogger<LogCleanerWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var cleaner = scope.ServiceProvider.GetRequiredService<ILogClearner>();

                try
                {
                    await cleaner.CleanAsync();
                    _logger.LogInformation("Log cleanup successful at {Time}", DateTime.UtcNow);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Log cleanup failed at {Time}", DateTime.UtcNow);
                }

                await Task.Delay(TimeSpan.FromHours(12), stoppingToken); // Run every 12 hours
            }
        }
    }
}
