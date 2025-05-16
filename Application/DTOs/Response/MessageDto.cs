namespace Application.DTOs.Response
{
    public class MessageDto
    {
        public Guid Id { get; set; }

        public string SenderId { get; set; }
        public virtual ApplicationUserDto Sender { get; set; }
        public string ReceiverId { get; set; }
        public virtual ApplicationUserDto Receiver { get; set; }

        public string Text { get; set; }
        public string Status { get; set; }
        public DateTime SentAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public List<FileMetadataDto>? Attachments { get; set; }
    }
}
