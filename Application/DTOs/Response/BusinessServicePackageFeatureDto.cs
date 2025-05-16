namespace Application.DTOs.Response
{
    public class BusinessServicePackageFeatureDto
    {
        public Guid Id { get; set; }
        public virtual BusinessServicePackageDto BusinessServicePackage { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string BillingCycle { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
