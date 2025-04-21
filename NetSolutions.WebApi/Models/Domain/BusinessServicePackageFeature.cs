
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NetSolutions.Models.Enums;

namespace NetSolutions.WebApi.Models.Domain;

public class BusinessServicePackageFeature
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid BusinessServicePackageId { get; set; }
    [ForeignKey(nameof(BusinessServicePackageId))]
    public virtual BusinessServicePackage BusinessServicePackage { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    public BillingCycle BillingCycle { get; set; } = BillingCycle.None;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
