using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class BusinessService
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string? ThumbnailId { get; set; }
        [ForeignKey(nameof(ThumbnailId))]
        public virtual FileMetadata Thumbnail { get; set; }

        public virtual List<BusinessServicePackage> BusinessServicePackages { get; set; }
        public virtual List<BusinessService_FileMetadata_Images> BusinessService_FileMetadata_Images { get; set; }
        public virtual List<BusinessService_Testimonial> BusinessService_Testimonials { get; set; }
    }

}

