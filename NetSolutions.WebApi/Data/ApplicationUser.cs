using Microsoft.AspNetCore.Identity;
using NetSolutions.WebApi.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NetSolutions.WebApi.Data;

public class ApplicationUser: IdentityUser
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string? Gender { get; set; }
    public string? Bio { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public string? FileMetadataId { get; set; }
    [ForeignKey(nameof(FileMetadataId))]
    public virtual FileMetadata ProfileImage { get; set; }

    [JsonIgnore]
    public override string PasswordHash { get; set; } = "";
    [JsonIgnore]
    public override string SecurityStamp { get; set; } = "";
    [JsonIgnore]
    public override string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

    public Guid? OrganizationId { get; set; }
    [ForeignKey(nameof(OrganizationId))]
    public virtual Organization? Organization { get; set; }

    public virtual List<UserActivity> UserActivities { get; set; }
    public virtual List<User_Solution> User_Solutions { get; set; }
}
