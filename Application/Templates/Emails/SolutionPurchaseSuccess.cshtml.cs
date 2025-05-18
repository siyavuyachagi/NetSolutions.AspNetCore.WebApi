using Application.DTOs.Response;

namespace Application.Templates.Emails
{
    public class SolutionPurchaseSuccess
    {
        public SolutionPurchaseSuccess(
            ApplicationUserDto user,
            BusinessProfileDto businessProfile,
            DateTime? expirationTime = null)
        {
            //User details 
            LastName = user.LastName;
            FirstName = user.FirstName;
            UserName = user.UserName;
            CreatedAt = user.CreatedAt;
            // Confirmation info
            this.ExpirationTime = expirationTime;
            // Company Info
            this.CompanyName = businessProfile.Name;
            this.LogoUrl = businessProfile.LogoUrl;
            this.SupportEmail = businessProfile.SupportEmail;
            this.SocialLinks = businessProfile.SocialLinks;
            this.CompanyAddress = businessProfile.PhysicalAddress;
            this.UnsubscribeUrl = businessProfile.UnsubscribeUrl;
        }

        // User
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        // Confirmation info
        public DateTime? ExpirationTime { get; set; }
        // Company
        public string CompanyName { get; set; }
        public string? LogoUrl { get; set; }
        public string SupportEmail { get; set; }
        public List<SocialLinkDto>? SocialLinks { get; set; }
        public PhysicalAddressDto CompanyAddress { get; set; }
        public string? UnsubscribeUrl { get; set; }
    }
}
