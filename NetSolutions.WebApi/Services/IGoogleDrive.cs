using System;
using System.IO;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Upload;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

using NetSolutions.Helpers;
using NetSolutions.WebApi.Models.Domain;


namespace NetSolutions.Services;


public interface IGoogleDrive
{
    Task<Result> CreateAsync(IFormFile file);
    Task<Result> DeleteAsync(string fileId);
    Task<Result> GetAsync(string fileId);
}

public class GoogleDrive : IGoogleDrive
{
    private readonly DriveService _driveService;
    private readonly GoogleDriveCredentials _driveCredentials;

    public GoogleDrive(IOptions<GoogleDriveCredentials> credentials)
    {
        _driveCredentials = credentials.Value;

        // Load the service account credentials from the JSON key file
        var credential = GoogleCredential.FromFile(_driveCredentials.CredentialsPath)
            .CreateScoped(DriveService.ScopeConstants.DriveFile);

        // Initialize the DriveService with the service account credentials
        _driveService = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "NetSolutions Drive Service"
        });
    }


    public async Task<Result> CreateAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return Result.Failed("Invalid file.");
        try
        {
            using (var stream = file.OpenReadStream())
            {
                // New file name
                string newFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = newFileName,
                    MimeType = file.ContentType,
                    Parents = new List<string> { _driveCredentials.ParentFolderId }
                };

                // Create the file in Google Drive
                var request = _driveService.Files.Create(fileMetadata, stream, file.ContentType);
                request.Fields = "id, name, size, webViewLink, webContentLink, fileExtension";

                var response = await request.UploadAsync();

                if (response.Status == UploadStatus.Completed)
                {
                    if (!string.IsNullOrEmpty(request.ResponseBody?.Id))
                    {
                        // Set file permissions after creation
                        var permission = new Permission()
                        {
                            Type = "anyone",
                            Role = "reader"
                        };

                        await _driveService.Permissions.Create(permission, request.ResponseBody.Id).ExecuteAsync();

                        var fileResource = new GoogleDriveResponse
                        {
                            Id = request.ResponseBody.Id,
                            Name = newFileName,
                            ContentType = file.ContentType,
                            Size = request.ResponseBody?.Size ?? file.Length,
                            Extension = request.ResponseBody?.FileExtension ?? Path.GetExtension(file.Name),
                            WebViewLink = request.ResponseBody?.WebViewLink,
                            WebContentLink = request.ResponseBody?.WebContentLink,
                            Provider = FileMetadata.EStorageProvider.GoogleDrive
                        };

                        return Result.Success(fileResource); // Return virtual path with file ID
                    }
                    return Result.Failed("Upload succeeded but file ID is missing.");
                }
                else
                {
                    return Result.Failed("Upload failed: ",response.Exception.Message);
                }
            }
        }
        catch (Exception ex)
        {
            return Result.Failed($"Error uploading file: {ex.Message}");
        }
    }


    public async Task<Result> DeleteAsync(string fileId)
    {
        try
        {
            await _driveService.Files.Delete(fileId).ExecuteAsync();
            return Result.Success("File deleted successfully.");
        }
        catch (Exception ex)
        {
            return Result.Failed($"Error deleting file: {ex.Message}");
        }
    }

    public async Task<Result> GetAsync(string fileId)
    {
        try
        {
            var request = _driveService.Files.Get(fileId);
            request.Fields = "id, name, size, webViewLink, webContentLink";

            var file = await request.ExecuteAsync();
            return Result.Success($"File found: {file.Name}, View: {file.WebViewLink}");
        }
        catch (Exception ex)
        {
            return Result.Failed($"Error retrieving file: {ex.Message}");
        }
    }
}


public class GoogleDriveCredentials
{
    public string CredentialsPath { get; set; }
    public string ParentFolderId { get; set; }
    public string VirtualUrl { get; set; }
}


public class GoogleDriveResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string ContentType { get; set; }
    public long Size { get; set; }
    public string WebViewLink { get; set; }
    public string WebContentLink { get; set; }
    public string Extension { get; set; }
    public FileMetadata.EStorageProvider Provider { get; set; } = FileMetadata.EStorageProvider.GoogleDrive;
}