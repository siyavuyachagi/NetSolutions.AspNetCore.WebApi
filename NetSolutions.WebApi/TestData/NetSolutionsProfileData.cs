using Microsoft.EntityFrameworkCore;
using NetSolutions.WebApi.Models.Domain;

namespace NetSolutions.WebApi.TestData;

public class NetSolutionsProfileData
{
    public static void GenerateBusinessProfileData(ModelBuilder builder)
    {
        try
        {
            var physicalAddress = new PhysicalAddress
            {
                Id = Guid.NewGuid(),
                Street = "Tondville, Silangwe location",
                City = "Bizana",
                Province = "Eastern Cape",
                Country = "South Africa",
                PostalCode = "4800",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            Seed.PhysicalAddresses.Add(physicalAddress);
            builder.Entity<PhysicalAddress>().HasData(physicalAddress);


            var profile = new NetSolutionsProfile
            {
                Id = Guid.NewGuid(),
                Name = "NetSolutions",
                Description = "NetSolutions is a technology solutions provider specializing in web development, web design, web hosting, database hosting, and graphic design services. Founded by CeeJay, it aims to deliver secure, efficient, and visually appealing digital solutions with reliable hosting services.",
                FoundAt = new DateTime(2025, 2, 1),
                RegistrationNumber = "NET-2025-0001",
                Email = "netsolutions2025@outlook.com",
                SupportEmail = "support@netsolutions.dev",
                Website = "https://netsolutions.co.za",
                HelpCenter = "https://help.netsolutions.dev",
                LogoUrl = "https://res.cloudinary.com/dw6sdelov/image/upload/v1744764229/logo_ep739f.png",
                PhysicalAddressId = physicalAddress.Id,
                UnsubscribeUrl = "https://netsolutions/newsfeed/unsubscribe"
            };
            Seed.Organizations.Add(profile);
            builder.Entity<NetSolutionsProfile>().HasData(profile);

            var socialLinks = new List<SocialLink>
            {
                new SocialLink { Id = Guid.NewGuid(),Handler="@netsolutions", Name = "Twitter", Url = "https://twitter.com/netsolutions", OrganizationId = profile.Id },
                new SocialLink { Id = Guid.NewGuid(),Handler="@netsolutions", Name = "Facebook", Url = "https://facebook.com/netsolutions", OrganizationId = profile.Id },
                new SocialLink { Id = Guid.NewGuid(),Handler="@netsolutions", Name = "LinkedIn", Url = "https://linkedin.com/company/netsolutions", OrganizationId = profile.Id }
            };
            Seed.SocialLinks.AddRange(socialLinks);
            builder.Entity<SocialLink>().HasData(socialLinks);

            Console.WriteLine($"Business Profile generated: 1");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating NetSolutionsProfile: {ex.Message}");
            throw;
        }
    }
}


