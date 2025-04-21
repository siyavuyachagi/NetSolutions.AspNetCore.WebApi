using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NetSolutions.WebApi.Models.DTOs;

public class OrganizationDto
{
    public Guid Id { get; set; }
    public virtual PhysicalAddress PhysicalAddress { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public virtual List<ApplicationUser> Users { get; set; }
    public virtual List<SocialLink> SocialLinks { get; set; }
}
