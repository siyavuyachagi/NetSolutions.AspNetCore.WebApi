using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class MobileApplicationProject : Project
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

}

