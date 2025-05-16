using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request
{
    public class PurchaseSolutionRequest
    {
        [Required]
        public string LastName { get; set; }
        public string FirstName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? OrganizationName { get; set; }
    }
}
