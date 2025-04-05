using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSolutions.WebApi.Models.Domain;

public class FileMetadata
{
    // Enum to define storage providers
    public enum EStorageProvider
    {
        [Display(Name = "Local Storage")]
        Local = 0,

        [Display(Name = "Google Drive")]
        GoogleDrive = 1,

        [Display(Name = "OneDrive")]
        OneDrive = 2,

        [Display(Name = "Amazon S3")]
        AWS_S3 = 3,

        [Display(Name = "Cloudinary")]
        Cloudinary = 4
    }

    [Key]
    public string Id { get; set; }

    public string Name { get; set; }
    public string ContentType { get; set; }
    public string Extension { get; set; }

    [Range(0, 1073741824)] // Max 1GB in binary prefixes or bytes
    public long Size { get; set; }

    public string? ViewLink { get; set; }
    public string? DownloadLink { get; set; }

    // Storage Provider (Google Drive, OneDrive, Local, etc.)
    public EStorageProvider StorageProvider { get; set; } = EStorageProvider.Local;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
