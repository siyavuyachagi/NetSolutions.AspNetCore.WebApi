using NetSolutions.WebApi.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetSolutions.WebApi.Models.Domain;

public class PhysicalAddress
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string Street { get; set; }
    public string City { get; set; }
    public string? Province { get; set; }
    public string? Country { get; set; }
    public string PostalCode { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual List<ApplicationUser> Users { get; set; }
    public virtual List<Organization> Organizations { get; set; }
}
