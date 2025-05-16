using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class User_Solution_PaymentTransaction
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }

        public Guid SolutionId { get; set; }
        [ForeignKey(nameof(SolutionId))]
        public virtual Solution Solution { get; set; }

        public Guid PaymentTransactionId { get; set; }
        [ForeignKey(nameof(PaymentTransactionId))]
        public virtual PaymentTransaction PaymentTransaction { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}

