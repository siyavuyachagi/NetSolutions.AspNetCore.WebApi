namespace Application.DTOs
{
    public class FileDownloadResult
    {
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }

        public FileDownloadResult(byte[] content, string contentType, string fileName)
        {
            Content = content;
            ContentType = contentType;
            FileName = fileName;
        }
    }

}
