namespace NetSolutions.WebApi.Services;
public interface ITimeLogger: IHostedService, IDisposable
{
}

public class TimeLogger : ITimeLogger
{
    private readonly ILogger<TimeLogger> _logger;
    private Timer _timer;

    public TimeLogger(ILogger<TimeLogger> logger)
    {
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("TimeLogger started.");
        _timer = new Timer(LogTime, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        return Task.CompletedTask;
    }

    private void LogTime(object state)
    {
        _logger.LogInformation("Current time: {time}", DateTime.Now);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("TimeLogger stopping.");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}

