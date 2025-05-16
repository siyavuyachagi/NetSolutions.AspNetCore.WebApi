using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Subscription
    {
        public enum EStatus
        {
            Pending,
            Active,
            Expired,
            Cancelled
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string ClientId { get; set; }
        [ForeignKey(nameof(ClientId))]
        public virtual Client Client { get; set; }

        public Guid BusinessServicePackageId { get; set; }
        [ForeignKey(nameof(BusinessServicePackageId))]
        public virtual BusinessServicePackage BusinessServicePackage { get; set; }

        public int RecurringCycle { get; set; } = 0; // Number of months subscription should run 0 = Infinite
        public EStatus Status { get; set; } = EStatus.Active;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

}

