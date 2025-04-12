namespace NetSolutions.WebApi.Models.DTOs;

public class SubscriptionDto
{
    public Guid Id { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int RecurringCycle { get; set; }
    public ClientDto Client { get; set; }
    public BusinessServicePackageDto BusinessServicePackage { get; set; }
}
