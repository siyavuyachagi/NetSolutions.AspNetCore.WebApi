using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class BusinessServicePackage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid BusinessServiceId { get; set; }
        [ForeignKey(nameof(BusinessServiceId))]
        public virtual BusinessService BusinessService { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public string BillingCycle { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }

        public virtual List<BusinessServicePackageFeature> BusinessServicePackageFeatures { get; set; }
        public virtual List<Subscription> Subscriptions { get; set; }
    }

}

