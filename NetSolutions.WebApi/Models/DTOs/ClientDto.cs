using NetSolutions.WebApi.Models.Domain;

namespace NetSolutions.WebApi.Models.DTOs;

public class ClientDto
{
    public string Id { get; set; }
    public string UserName { get; set; } = default!;
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public string Email { get; set; } = default!;
    public bool EmailConfirmed { get; set; }
    public string? PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public string? Gender { get; set; }
    public string? Bio { get; set; }
    public string? Avatar { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<string> Roles { get; set; } = new();
    public Subscription? Subscription { get; set; }
    public OrganizationDto? Organization { get; set; }
    public List<ProjectDto> Projects { get; set; } = new();
    public List<UserActivity> UserActivities { get; set; } = new();
    public List<SolutionDto> Solutions { get; set; } = new();
}
