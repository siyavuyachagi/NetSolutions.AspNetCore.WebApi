using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NetSolutions.WebApi.Data;


namespace NetSolutions.WebApi.Models.Domain;

public class UserActivity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser User { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
