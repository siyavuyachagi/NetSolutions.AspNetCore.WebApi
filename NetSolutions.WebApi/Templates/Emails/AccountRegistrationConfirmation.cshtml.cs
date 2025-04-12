using NetSolutions.WebApi.Models.Domain;

namespace NetSolutions.Templates.Emails;


public class AccountRegistrationConfirmation
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string LogoUrl { get; set; }
    public string CompanyName { get; set; }
    public string CompanyAddress { get; set; }
    public string UserName { get; set; }
    public string ConfirmationUrl { get; set; }
    public string ConfirmationCode { get; set; }
    public DateTime ExpirationTime { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string SupportEmail { get; set; }
    public List<SocialLink> SocialLinks { get; set; }
    public string UnsubscribeUrl { get; set; }
}
