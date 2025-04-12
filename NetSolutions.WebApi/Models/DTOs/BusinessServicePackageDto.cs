using NetSolutions.WebApi.Models.Domain;

namespace NetSolutions.WebApi.Models.DTOs;

public class BusinessServicePackageDto
{
    public Guid Id { get; set; }
    public Guid BusinessServiceId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string BillingCycle { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public BusinessService BusinessService { get; set; }
    public List<BusinessServicePackageFeature> BusinessServicePackageFeatures { get; set; }
    public List<SubscriptionDto> Subscriptions { get; set; }
}
