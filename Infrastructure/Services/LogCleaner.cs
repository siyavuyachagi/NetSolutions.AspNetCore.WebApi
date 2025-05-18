using Application.Interfaces;

namespace Infrastructure.Services
{
    public class LogCleaner : ILogClearner
    {
        private readonly string[] _logDirectories =
        {
            "Logs/Info",
            "Logs/Warnings",
            "Logs/Errors",
            "Logs/Debug"
        };

        public async Task CleanAsync()
        {
            foreach (var dir in _logDirectories)
            {
                if (!Directory.Exists(dir)) continue;

                var files = Directory.GetFiles(dir);
                foreach (var file in files)
                {
                    try
                    {
                        var fileInfo = new FileInfo(file);
                        if (fileInfo.CreationTime < DateTime.Now.AddDays(-7)) // delete files older than 7 days
                        {
                            fileInfo.Delete();
                        }
                    }
                    catch
                    {
                        // optional: log if you want to catch exceptions here
                    }
                }
            }

            await Task.CompletedTask;
        }
    }
}
