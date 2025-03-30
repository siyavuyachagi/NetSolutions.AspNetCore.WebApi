using NetSolutions.WebApi.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetSolutions.WebApi.Models.Domain;

public class Message
{
    public enum EStatus
    {
        Pending,
        Sent,
        Seen,
    }

    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string SenderId { get; set; }
    [ForeignKey(nameof(SenderId))]
    public virtual ApplicationUser Sender { get; set; }
    public string Text { get; set; }
    public EStatus Status { get; set; } = EStatus.Pending;
    public DateTime SentAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
