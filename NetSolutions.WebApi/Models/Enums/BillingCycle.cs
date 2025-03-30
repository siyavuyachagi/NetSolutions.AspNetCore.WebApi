using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSolutions.Models.Enums;

public enum BillingCycle
{
    None, // No billing cycle selected
    [Display(Name ="Fourth night")]
    FourthNight,   // Every 2 weeks
    Weekly,        // Every 7 days
    [Display(Name ="Monthly")]
    Monthly,       // Every month
    Quarterly,     // Every 3 months
    Yearly         // Every year
}

