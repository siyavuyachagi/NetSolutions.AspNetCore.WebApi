using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSolutions.Models.Enums;

namespace NetSolutions.WebApi.Models.Domain;

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
    public BillingCycle BillingCycle { get; set; } = BillingCycle.Monthly;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public virtual List<BusinessServicePackageFeature> PackageFeatures { get; set; }
    public virtual List<Subscription> Subscriptions { get; set; }
}
