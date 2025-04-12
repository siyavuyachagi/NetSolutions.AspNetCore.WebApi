using NetSolutions.Templates.Emails;
using NetSolutions.WebApi.Models.Domain;

namespace NetSolutions.WebApi.TestData;

public class EmailTemplatesData
{
    // Sample data creation methods
    public static AccountRegistrationConfirmation CreateSampleConfirmationModel()
    {
        return new AccountRegistrationConfirmation
        {
            LastName = "Doe",
            FirstName = "John",
            UserName = "john.doe@example.com",
            CompanyName = "NetSolutions",
            LogoUrl = "/images/logo.png",
            RegistrationDate = DateTime.Now,
            ConfirmationUrl = "https://yourapp.com/confirm?token=sample-token",
            ConfirmationCode = "ABC123XYZ",
            ExpirationTime = DateTime.Now.AddHours(24),
            SupportEmail = "support@netsolutions.com",
            SocialLinks = new List<SocialLink>
            {
                new SocialLink { Name = "Twitter", Url = "https://twitter.com/netsolutions" },
                new SocialLink { Name = "Facebook", Url = "https://facebook.com/netsolutions" }
            },
            CompanyAddress = "123 Main St, Anytown, AT 12345",
            UnsubscribeUrl = "https://yourapp.com/unsubscribe?email=john.doe@example.com"
        };
    }

    public static AccountRegistrationSuccessful CreateSampleSuccessModel()
    {
        return new AccountRegistrationSuccessful
        {
            LastName = "Doe",
            FirstName = "John",
            UserName = "john.doe@example.com",
            CompanyName = "NetSolutions",
            LogoUrl = "/images/logo.png",
            RegistrationDate = DateTime.Now,
            LoginUrl = "https://yourapp.com/confirm?token=sample-token",
            SupportEmail = "support@netsolutions.com",
            SocialLinks = new List<SocialLink>
            {
                new SocialLink { Name = "Twitter", Url = "https://twitter.com/netsolutions" },
                new SocialLink { Name = "Facebook", Url = "https://facebook.com/netsolutions" }
            },
            CompanyAddress = "123 Main St, Anytown, AT 12345",
            UnsubscribeUrl = "https://yourapp.com/unsubscribe?email=john.doe@example.com"
        };
    }
}
