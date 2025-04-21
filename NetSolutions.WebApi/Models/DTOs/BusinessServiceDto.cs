using NetSolutions.WebApi.Models.Domain;

namespace NetSolutions.WebApi.Models.DTOs;

public class BusinessServiceDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public List<TestimonialDto> Testimonials { get; set; }
    public string Thumbnail { get; set; }
    public List<BusinessServicePackageDto> BusinessServicePackages { get; set; }
}
