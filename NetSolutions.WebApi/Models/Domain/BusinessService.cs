using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSolutions.WebApi.Models.Domain;

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
    public virtual List<BusinessService_Testimonial> Testimonials { get; set; }
}
