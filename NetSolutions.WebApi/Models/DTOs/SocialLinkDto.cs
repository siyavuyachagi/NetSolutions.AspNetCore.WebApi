using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetSolutions.WebApi.Models.DTOs
{
    public class SocialLinkDto
    {
        public Guid Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Organization Organization { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Handler { get; set; }
        public string IconHTML { get; set; }
        public string IconUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
