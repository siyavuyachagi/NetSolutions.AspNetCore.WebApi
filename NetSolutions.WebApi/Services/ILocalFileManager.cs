
using NetSolutions.Helpers;
using NetSolutions.WebApi.Models.Domain;
using System.IO.Compression;

namespace NetSolutions.Services;

public interface ILocalFileManager
{
    Task<Result> CreateAsync(IFormFile file, string? relativePath = null); // Accepts a parameter
    Task<Result> GetDetailsAsync(string virtualPath);  // More descriptive naming
    Task<Result> DeleteAsync(string virtualPath);
    Task<Result> ZipAsync(List<IFormFile> files);
    Task<Result> ZipAsync(IFormFile files);
    Task<Result> UnzipAsync(string virtualPath);
}


public class LocalFileManager : ILocalFileManager
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<LocalFileManager> _logger;
    private readonly string _relativePath = "wwwroot";

    public LocalFileManager(
        IWebHostEnvironment webHostEnvironment,
        ILogger<LocalFileManager> logger
    )
    {
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
    }

    public async Task<Result> CreateAsync(IFormFile file, string? relativePath = null)
    {
        if (file == null || file.Length == 0)
            return Result.Failed("File cannot be null or empty");

        try
        {
            // Normalize path to prevent directory traversal issues
            relativePath ??= _relativePath;
            relativePath = relativePath.TrimStart('/').TrimStart('\\');

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder); // Ensure directory exists

            // Generate a unique file name
            string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string physicalPath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(physicalPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Generate web-accessible virtual path
            string virtualPath = $"/{relativePath}/{uniqueFileName}";

            var response = new FileCreateResponse
            {
                FileName = uniqueFileName,
                ContentType = file.ContentType,
                RelativePath = relativePath,
                PhysicalPath = physicalPath, // Physical path
                VirtualPath = virtualPath // Web-accessible path
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            _logger.LogError($"File upload failed: {ex.Message}");
            return Result.Failed($"Error uploading file: {ex.Message}");
        }
    }



    public async Task<Result> GetDetailsAsync(string virtualPath)
    {
        if (string.IsNullOrEmpty(virtualPath))
            return Result.Failed("Invalid file path");

        string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, _relativePath, virtualPath);

        if (!File.Exists(fullPath))
            return Result.Failed("File not found");

        await Task.Delay(100); // Simulated async operation

        return Result.Success(new { Path = fullPath, Name = Path.GetFileName(fullPath) });
    }

    public async Task<Result> DeleteAsync(string virtualPath)
    {
        if (string.IsNullOrEmpty(virtualPath))
            return Result.Failed("Invalid file path");

        string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, _relativePath, virtualPath);

        if (!File.Exists(fullPath))
            return Result.Failed("File not found");

        try
        {
            File.Delete(fullPath);
            await Task.Delay(100); // Simulated async operation
            return Result.Success("File deleted successfully.");
        }
        catch (Exception ex)
        {
            return Result.Failed($"Error deleting file: {ex.Message}");
        }
    }

    public async Task<Result> ZipAsync(List<IFormFile> files)
    {
        if (files == null || files.Count == 0)
            return Result.Failed("No files provided.");

        string zipPath = Path.Combine(Path.GetTempPath(), $"archive_{Guid.NewGuid()}.zip");

        try
        {
            using (var zipStream = new FileStream(zipPath, FileMode.Create))
            {
                using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in files)
                    {
                        var entry = archive.CreateEntry(file.FileName, CompressionLevel.Optimal);
                        using (var entryStream = entry.Open())
                        using (var fileStream = file.OpenReadStream())
                        {
                            await fileStream.CopyToAsync(entryStream);
                        }
                    }
                }
            }
            return Result.Success(zipPath);
        }
        catch (Exception ex)
        {
            return Result.Failed($"Error creating ZIP: {ex.Message}");
        }
    }

    public async Task<Result> ZipAsync(IFormFile file)
    {
        return await ZipAsync(new List<IFormFile> { file });
    }

    public async Task<Result> UnzipAsync(string zipFilePath)
    {
        if (!File.Exists(zipFilePath))
            return Result.Failed("ZIP file not found.");

        string extractPath = Path.Combine(Path.GetTempPath(), $"unzipped_{Guid.NewGuid()}");

        try
        {
            ZipFile.ExtractToDirectory(zipFilePath, extractPath);
            return Result.Success(extractPath);
        }
        catch (Exception ex)
        {
            return Result.Failed($"Error extracting ZIP: {ex.Message}");
        }
    }
}





public class FileCreateResponse
{
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public string RelativePath { get; set; }
    public string VirtualPath { get; set; }
    public string PhysicalPath { get; set; }
    public long Size { get; set; }
    public string Extension { get; set; }
    public FileMetadata.EStorageProvider StorageProvider { get; set; } = FileMetadata.EStorageProvider.Local;
}