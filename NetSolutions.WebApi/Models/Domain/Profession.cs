
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NetSolutions.WebApi.Models.Domain;

public class Profession
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    // Education and certification requirements
    [StringLength(250)]
    public string EducationRequirements { get; set; }

    [StringLength(250)]
    public string CertificationRequirements { get; set; }

    [StringLength(500)]
    public string CoreCompetencies { get; set; }

    // Career data
    [Column(TypeName = "decimal(18,2)")]
    public decimal? AverageSalary { get; set; }

    public int? ExperienceYearsRequired { get; set; }

    [StringLength(250)]
    public string CareerPath { get; set; }

    // Industry information
    [StringLength(100)]
    public string Industry { get; set; }

    // Status and metadata
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual List<Staff> Professionals { get; set; }
}
