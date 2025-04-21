using NetSolutions.WebApi.Models.Domain;

namespace NetSolutions.WebApi.Models.DTOs;

public class ApplicationUserDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public string PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public string Gender { get; set; }
    public string Bio { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Avatar { get; set; }
    public List<string> UserRoles { get; set; }
    public OrganizationDto? Organization { get; set; }
    public PhysicalAddressDto PhysicalAddress { get; set; }
    public List<UserActivityDto> UserActivities { get; set; }
    public virtual string Discriminator { get; set; }

}
