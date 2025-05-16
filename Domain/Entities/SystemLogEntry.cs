using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    /// <summary>
    /// Class representing a log entry in the database
    /// </summary>
    public class SystemLogEntry
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public LogLevel Level { get; set; }
        [MaxLength(255)]
        public string Message { get; set; }
        [MaxLength(500)]
        public string? Source { get; set; }
        [MaxLength(4000)]
        public string? Exception { get; set; }
        [MaxLength(4000)]
        public string? StackTrace { get; set; }

        public virtual List<FileMetadata> Screenshorts { get; set; }
    }
}

