using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
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

}

