using Application.DTOs.Response;

namespace Infrastructure.Templates.Emails
{
    public class AccountRegistrationConfirmation
    {
        public AccountRegistrationConfirmation(
            ApplicationUserDto user,
            BusinessProfileDto netSolutionsProfile,
            string confirmationUrl,
            string confirmationCode,
            DateTime? expirationTime = null)
        {
            //User details 
            LastName = user.LastName;
            FirstName = user.FirstName;
            UserName = user.UserName;
            CreatedAt = user.CreatedAt;
            // Confirmation info
            this.ConfirmationUrl = confirmationUrl;
            //this.ConfirmationCode = confirmationCode;
            this.ExpirationTime = expirationTime;
            // Company Info
            this.CompanyName = netSolutionsProfile.Name;
            this.LogoUrl = netSolutionsProfile.LogoUrl;
            this.SupportEmail = netSolutionsProfile.SupportEmail;
            this.SocialLinks = netSolutionsProfile.SocialLinks;
            this.CompanyAddress = netSolutionsProfile.PhysicalAddress;
            this.UnsubscribeUrl = netSolutionsProfile.UnsubscribeUrl;
        }

        // User
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        // Confirmation info
        public string ConfirmationUrl { get; set; }
        public string? ConfirmationCode { get; set; }
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

