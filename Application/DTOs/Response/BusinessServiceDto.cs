

namespace Application.DTOs.Response
{
    public class BusinessServiceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public string Thumbnail { get; set; }
        public List<TestimonialDto> Testimonials { get; set; }
        public List<FileMetadataDto> Images { get; set; }
        public List<BusinessServicePackageDto> BusinessServicePackages { get; set; }
    }
}

