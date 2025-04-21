using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSolutions.WebApi.Models.Domain;

public class Project
{
    public enum EStatus
    {
        [Display(Name = "Not Started")]
        NotStarted = 0, // The project has been created but work has not begun

        [Display(Name = "Planning Phase")]
        Planning = 1, // Gathering requirements and defining the project scope

        [Display(Name = "In Progress")]
        InProgress = 2, // Development work is actively taking place

        [Display(Name = "On Hold")]
        OnHold = 3, // The project is temporarily paused

        [Display(Name = "Awaiting Approval")]
        AwaitingApproval = 4, // Waiting for client or management approval

        [Display(Name = "Testing & QA")]
        Testing = 5, // Quality assurance and testing phase

        [Display(Name = "Deployment Phase")]
        Deployment = 6, // The project is being deployed to production

        [Display(Name = "Maintenance & Support")]
        Maintenance = 7, // Post-deployment maintenance and support

        [Display(Name = "Completed Successfully")]
        Completed = 8, // The project is finished successfully

        [Display(Name = "Cancelled/Terminated")]
        Cancelled = 9 // The project has been abandoned or terminated
    }


    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string ClientId { get; set; }
    [ForeignKey(nameof(ClientId))]
    public virtual Client Client { get; set; }

    public Guid BusinessServiceId { get; set; }
    [ForeignKey(nameof(BusinessServiceId))]
    public virtual BusinessService BusinessService { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Budget { get; set; } = 0;
    public EStatus Status { get; set; } = EStatus.NotStarted;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
    public string? TargetAudience { get; set; }
    public virtual string? Discriminator { get; set; }

    public virtual ProjectTeam ProjectTeam { get; set; }
    public virtual Project_Timeline Project_Timeline { get; set; }

    public virtual List<Project_FileMetadata_Document> Project_FileMetadata_Documents { get; set; }
    public virtual List<Solution> Solutions { get; set; }
    public virtual List<Project_TechnologyStack> Project_TechnologyStacks { get; set; }
    public virtual List<ProjectMilestone> ProjectMilestones { get; set; }
    public virtual List<ProjectTask> ProjectTasks { get; set; }
}

