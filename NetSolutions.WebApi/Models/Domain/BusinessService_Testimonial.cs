using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSolutions.WebApi.Models.Domain;

public class BusinessService_Testimonial
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid BusinessServiceId { get; set; }
    [ForeignKey(nameof(BusinessServiceId))]
    public virtual BusinessService BusinessService { get; set; }

    public Guid TestimonialId { get; set; }
    [ForeignKey(nameof(TestimonialId))]
    public virtual Testimonial Testimonial { get; set; }
}
