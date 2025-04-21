using NetSolutions.Models.Enums;
using NetSolutions.WebApi.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetSolutions.WebApi.Models.DTOs
{
    public class BusinessServicePackageFeatureDto
    {
        public Guid Id { get; set; }
        public virtual BusinessServicePackageDto BusinessServicePackage { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public BillingCycle BillingCycle { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
