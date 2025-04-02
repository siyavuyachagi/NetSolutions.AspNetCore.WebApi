using NetSolutions.WebApi.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetSolutions.WebApi.Models.Domain;

public class Solution_Like
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid SolutionId { get; set; }
    [ForeignKey(nameof(SolutionId))]
    public virtual Solution Solution { get; set; }

    public string LikerId { get; set; }
    [ForeignKey(nameof(LikerId))]
    public virtual ApplicationUser Liker { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
