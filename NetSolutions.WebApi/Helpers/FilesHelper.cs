
namespace NetSolutions.Helpers;

public class FilesHelper
{
    // Helper method to get project root directory
    public static string GetProjectRootDirectory()
    {
        try
        {
            // Navigate up from the bin directory to reach the project root
            // Typically bin/Debug/net9.0/ -> need to go up 3 levels
            string binDirectory = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo directory = new DirectoryInfo(binDirectory);

            // Go up to bin parent
            if (directory.Parent != null && directory.Parent.Parent != null && directory.Parent.Parent.Parent != null)
            {
                return directory.Parent.Parent.Parent.FullName;
            }

            // Fallback: try a simpler approach by searching for the solution file
            directory = new DirectoryInfo(binDirectory);
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }

            return directory?.FullName ?? Directory.GetCurrentDirectory();
        }catch
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Error getting project root directory");
            return null;
        }
    }

    public static string GetContentType(string extension)
    {
        return extension.ToLower() switch
        {
            // Images
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",

            // Documents
            ".txt" => "text/plain",
            ".pdf" => "application/pdf",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",

            // Archives
            ".zip" => "application/zip",
            ".rar" => "application/vnd.rar",
            ".tar" => "application/x-tar",
            ".gz" => "application/gzip",
            ".7z" => "application/x-7z-compressed",

            // Code & Scripts
            ".cs" => "text/plain",  // C# source code
            ".java" => "text/plain", // Java source code
            ".cpp" => "text/plain", // C++ source code
            ".h" => "text/plain", // C/C++ header file
            ".py" => "text/x-python",
            ".js" => "application/javascript",
            ".ts" => "application/typescript",
            ".jsx" => "text/jsx",
            ".tsx" => "text/tsx",
            ".html" => "text/html",
            ".htm" => "text/html",
            ".css" => "text/css",
            ".scss" => "text/x-scss",
            ".less" => "text/x-less",
            ".php" => "application/x-httpd-php",
            ".rb" => "text/x-ruby",
            ".go" => "text/plain",
            ".swift" => "text/plain",
            ".kt" => "text/x-kotlin",
            ".sh" => "application/x-sh",
            ".bat" => "application/x-msdos-program",
            ".ps1" => "text/plain", // PowerShell script

            // Config Files
            ".json" => "application/json",
            ".xml" => "application/xml",
            ".yaml" => "text/yaml",
            ".yml" => "text/yaml",
            ".ini" => "text/plain",
            ".env" => "text/plain",
            ".properties" => "text/plain",

            // Data Files
            ".csv" => "text/csv",
            ".sql" => "application/sql",
            ".md" => "text/markdown",
            ".log" => "text/plain",

            // Miscellaneous
            ".exe" => "application/octet-stream",
            ".dll" => "application/octet-stream",
            ".iso" => "application/x-iso9660-image",

            _ => "application/octet-stream", // Default fallback
        };
    }

}
