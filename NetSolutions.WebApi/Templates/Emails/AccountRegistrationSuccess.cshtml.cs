using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Models.Domain;

namespace NetSolutions.WebApi.Templates.Emails
{
    public class AccountRegistrationSuccess
    {
        public AccountRegistrationSuccess(
            ApplicationUser user,
            NetSolutionsProfile netSolutionsProfile,
            string? dashboardUrl = null)
        {
            //User details 
            LastName = user.LastName;
            FirstName = user.FirstName;
            UserName = user.UserName;
            CreatedAt = user.CreatedAt;
            // Account info
            this.DashboardUrl = dashboardUrl;
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
        // Account info
        public string DashboardUrl { get; set; }
        // Company
        public string CompanyName { get; set; }
        public string? LogoUrl { get; set; }
        public string SupportEmail { get; set; }
        public List<SocialLink>? SocialLinks { get; set; }
        public PhysicalAddress CompanyAddress { get; set; }
        public string? UnsubscribeUrl { get; set; }
        public string HelpCenterUrl { get; set; }
    }

}
