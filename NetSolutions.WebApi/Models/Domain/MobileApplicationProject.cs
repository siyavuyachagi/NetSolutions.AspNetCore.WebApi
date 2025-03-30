using System.ComponentModel.DataAnnotations;

namespace NetSolutions.WebApi.Models.Domain;

public class MobileApplicationProject: Project
{
    public enum EPlatform
    {
        [Display(Name = "Android")]
        Android,
        [Display(Name = "IOS")]
        IOS,
        [Display(Name = "Cross Platform")]
        CrossPlatform
    }

    public EPlatform Platform { get; set; }
}
