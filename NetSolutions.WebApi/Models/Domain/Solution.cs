using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace NetSolutions.WebApi.Models.Domain;

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
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public string? SourceUrl { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    public double Version { get; set; } = 1.0;
    public bool IsDeleted { get; set; }

    public virtual User_Solution Buyer { get; set; }
    public virtual List<Solution_FileMetadata_Image> Images { get; set; }
    public virtual List<SolutionFeature> SolutionFeatures { get; set; }
    public virtual List<Solution_FileMetadata_Document> Documents { get; set; }
    public virtual List<Solution_Review> Reviews { get; set; }
    public virtual List<Solution_TechnologyStack> TechnologyStacks { get; set; }
}
