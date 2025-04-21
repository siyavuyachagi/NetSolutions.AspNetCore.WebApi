using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSolutions.WebApi.Models.Domain;

public class NetSolutionsProfile : Organization
{
    public string RegistrationNumber { get; set; }
    public DateTime FoundAt { get; set; }

    public string Email { get; set; }
    public string SupportEmail { get; set; }
    public string Website { get; set; }
    public string HelpCenter { get; set; }
    public string LogoUrl { get; set; }
    public string UnsubscribeUrl { get; set; }

    public PhysicalAddress PhysicalAddress { get; set; }
    public List<SocialLink> SocialLinks { get; set; }
}
