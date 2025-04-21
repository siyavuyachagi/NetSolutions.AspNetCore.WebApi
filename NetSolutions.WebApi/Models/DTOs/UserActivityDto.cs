using NetSolutions.WebApi.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetSolutions.WebApi.Models.DTOs
{
    public class UserActivityDto
    {
        public Guid Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
