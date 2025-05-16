using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Message
    {
        public Message()
        {

        }
        public Message(string senderId, string recieverId, string text, List<FileMetadata>? attachments = null)
        {
            SenderId = senderId;
            ReceiverId = recieverId;
            Text = text;
            Status = EStatus.Pending;
            IsDeleted = false;
            Attachments = attachments;
        }
        public enum EStatus
        {
            [Display(Name ="Pending")]
            Pending,
            [Display(Name ="Sent")]
            Sent,
            [Display(Name = "Seen")]
            Seen,
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string SenderId { get; set; }
        [ForeignKey(nameof(SenderId))]
        public virtual ApplicationUser Sender { get; set; }

        // Add recipient information
        public string ReceiverId { get; set; }
        [ForeignKey(nameof(ReceiverId))]
        public virtual ApplicationUser Receiver { get; set; }

        public string Text { get; set; }
        public EStatus Status { get; set; } = EStatus.Pending;
        public DateTime SentAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Optional: Add these for additional functionality
        public bool IsDeleted { get; set; } = false;
        public List<FileMetadata>? Attachments { get; set; } // For file attachments (can be a URL or file path)
    }
}

