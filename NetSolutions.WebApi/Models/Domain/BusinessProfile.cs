using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSolutions.WebApi.Models.Domain;

public class NetSolutionsProfile: Organization
{
    public string RegistrationNumber { get; set; }
    public string Description { get; set; }
    public DateTime FoundAt { get; set; }
}