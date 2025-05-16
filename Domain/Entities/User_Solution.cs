using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class User_Solution
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }

        public Guid SolutionId { get; set; }
        [ForeignKey(nameof(SolutionId))]
        public virtual Solution Solution { get; set; }

        public DateTime CreatedAt { get; set; }
    }

}

