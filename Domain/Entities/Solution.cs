using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Domain.Entities
{
    public class Solution
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid? ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public virtual Project Project { get; set; } = new Project();

        public Guid? ProjectTeamId { get; set; }
        [ForeignKey(nameof(ProjectTeamId))]
        public virtual ProjectTeam ProjectTeam { get; set; } = new ProjectTeam();

        public string Name { get; set; }
        public string Description { get; set; }
        public string? PreviewUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? SourceUrl { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public string Version { get; set; } = "1.0.0";
        public bool IsDeleted { get; set; }
        public virtual string? Discriminator { get; set; }

        public virtual List<SolutionFeature> SolutionFeatures { get; set; }
        public virtual List<User_Solution> User_Solutions { get; set; }
        public virtual List<Solution_FileMetadata_Image> Solution_FileMetadata_Images { get; set; }
        public virtual List<Solution_FileMetadata_Document> Solution_FileMetadata_Documents { get; set; }
        public virtual List<Solution_Review> Solution_Reviews { get; set; }
        public virtual List<Solution_TechnologyStack> Solution_TechnologyStacks { get; set; }
        public virtual List<Solution_Like> Solution_Likes { get; set; } = new();
        public virtual List<Solution_Bookmark> Solution_Bookmarks { get; set; }
    }
}

