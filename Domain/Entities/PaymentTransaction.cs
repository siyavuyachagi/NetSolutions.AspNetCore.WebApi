using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class PaymentTransaction
    {
        public enum EStatus
        {
            Pending,
            Complete,
            Successfull,
            Failed,
            Cancelled
        }
        [Key]
        public Guid Id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public EStatus Status { get; set; } = EStatus.Pending;
        public string? TransactionReference { get; set; }
        public string? AdditionalNotes { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

