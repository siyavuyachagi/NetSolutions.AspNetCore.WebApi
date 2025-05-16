namespace Application.DTOs.Response
{
    public class SystemLogEntryDto
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Level { get; set; } // string for better JSON serialization
        public string Message { get; set; }
        public string? Source { get; set; }
        public string? Exception { get; set; }
        public string? StackTrace { get; set; }
        public List<FileMetadataDto>? Screenshorts { get; set; }
    }
}
