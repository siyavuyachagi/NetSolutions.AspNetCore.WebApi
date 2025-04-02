using NetSolutions.WebApi.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetSolutions.WebApi.Models.Domain;

public class Solution_Bookmark
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid SolutionId { get; set; }
    [ForeignKey(nameof(SolutionId))]
    public virtual Solution Solution { get; set; }

    public string BookmakerId { get; set; }
    [ForeignKey(nameof(BookmakerId))]
    public virtual ApplicationUser Bookmaker { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
