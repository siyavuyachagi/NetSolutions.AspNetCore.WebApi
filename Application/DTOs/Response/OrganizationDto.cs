namespace Application.DTOs.Response
{
    public class OrganizationDto
    {
        public Guid Id { get; set; }
        public virtual PhysicalAddressDto PhysicalAddress { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        public string? SupportEmail { get; set; }
        public string? UnsubscribeUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public virtual List<ApplicationUserDto> Users { get; set; }
        public virtual List<SocialLinkDto> SocialLinks { get; set; }
    }
}

