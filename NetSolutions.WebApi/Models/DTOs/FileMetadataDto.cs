using System.ComponentModel.DataAnnotations;

namespace NetSolutions.WebApi.Models.DTOs
{
    public class FileMetadataDto
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string ContentType { get; set; }
        public string Extension { get; set; }

        public long Size { get; set; }

        public string? ViewLink { get; set; }
        public string? DownloadLink { get; set; }

        // Storage Provider (Google Drive, OneDrive, Local, etc.)
        public string StorageProvider { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
