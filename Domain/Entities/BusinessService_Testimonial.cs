using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
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

}

