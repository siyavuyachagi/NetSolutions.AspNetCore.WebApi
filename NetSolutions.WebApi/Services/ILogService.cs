using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Models.Domain;

namespace NetSolutions.Services;

// Interface for the logging service
public interface ILogService
{
    void Information(string message, string source = null);
    void Warning(string message, string source = null);
    void Error(string message, Exception exception = null, string source = null);
    void Critical(string message, Exception exception = null, string source = null);
}

// Implementation class
public class LogService : ILogService
{
    private readonly ApplicationDbContext _db;

    public LogService(ApplicationDbContext dbContext)
    {
        _db = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public void Information(string message, string source = null)
    {
        LogMessage(LogLevel.Information, message, source);
    }

    public void Warning(string message, string source = null)
    {
        LogMessage(LogLevel.Warning, message, source);
    }

    public void Error(string message, Exception exception = null, string source = null)
    {
        LogMessage(LogLevel.Error, message, source, exception);
    }

    public void Critical(string message, Exception exception = null, string source = null)
    {
        LogMessage(LogLevel.Critical, message, source, exception);
    }

    private void LogMessage(LogLevel level, string message, string source = null, Exception exception = null)
    {
        var logEntry = new SystemLogEntry
        {
            Timestamp = DateTime.UtcNow,
            Level = level,
            Message = message,
            Source = source ?? GetCallerInfo(),
            Exception = exception?.ToString(),
            StackTrace = exception?.StackTrace
        };

        try
        {
            // Log to database
            _db.SystemLogEntries.Add(logEntry);
            _db.SaveChanges();
        }
        catch (Exception ex)
        {
            // Fallback to console if database logging fails
            WriteToConsole(logEntry);
            WriteToConsole(new SystemLogEntry
            {
                Timestamp = DateTime.UtcNow,
                Level = LogLevel.Error,
                Message = $"Failed to write log to database: {ex.Message}",
                Source = nameof(LogService),
                Exception = ex.ToString()
            });
        }
    }

    private void WriteToConsole(SystemLogEntry entry)
    {
        var originalColor = Console.ForegroundColor;

        Console.ForegroundColor = entry.Level switch
        {
            LogLevel.Information => ConsoleColor.White,
            LogLevel.Warning => ConsoleColor.Yellow,
            LogLevel.Error => ConsoleColor.Red,
            LogLevel.Critical => ConsoleColor.DarkRed,
            _ => ConsoleColor.Gray
        };

        Console.WriteLine($"[{entry.Timestamp:yyyy-MM-dd HH:mm:ss}] [{entry.Level}] [{entry.Source}] {entry.Message}");

        if (!string.IsNullOrEmpty(entry.Exception))
        {
            Console.WriteLine($"Exception: {entry.Exception}");
        }

        Console.ForegroundColor = originalColor;
    }

    private string GetCallerInfo()
    {
        try
        {
            var frame = new System.Diagnostics.StackFrame(3, true);
            var method = frame.GetMethod();
            return $"{method?.DeclaringType?.FullName}.{method?.Name}";
        }
        catch
        {
            return "Unknown";
        }
    }
}

