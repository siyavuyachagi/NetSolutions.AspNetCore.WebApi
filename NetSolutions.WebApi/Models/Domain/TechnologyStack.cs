using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSolutions.WebApi.Models.Domain;

public class TechnologyStack
{
    public enum EType
    {
        [Display(Name = "AI/ML Framework")]
        AIMLFramework,
        [Display(Name = "Analytics Tool")]
        AnalyticsTool,
        [Display(Name = "API")]
        API,
        [Display(Name = "Authentication Framework")]
        AuthenticationFramework,
        [Display(Name = "Build Tool")]
        BuildTool,
        [Display(Name = "CI/CD Tool")]
        CICDTool,
        [Display(Name = "Cloud Computing")]
        CloudComputing,
        [Display(Name = "CMS")]
        CMS,
        [Display(Name = "Code Hosting")]
        CodeHosting,
        [Display(Name = "Code Quality Tool")]
        CodeQualityTool,
        [Display(Name = "Communication Protocol")]
        CommunicationProtocol,
        [Display(Name = "Containerization & Orchestration")]
        ContainerizationOrchestration,
        [Display(Name = "Database")]
        Database,
        [Display(Name = "Design Tool")]
        DesignTool,
        [Display(Name = "DevOps Tool")]
        DevOpsTool,
        [Display(Name = "Documentation Tool")]
        DocumentationTool,
        [Display(Name = "Framework")]
        Framework,
        [Display(Name = "IDEs")]
        IDEs,
        [Display(Name = "Infrastructure as Code")]
        InfrastructureAsCode,
        [Display(Name = "Library")]
        Library,
        [Display(Name = "Logging & Monitoring")]
        LoggingMonitoring,
        [Display(Name = "Message Queue")]
        MessageQueue,
        [Display(Name = "Mobile Development")]
        MobileDevelopment,
        [Display(Name = "ORM")]
        ORM,
        [Display(Name = "Other")]
        Other,
        [Display(Name = "Package Manager")]
        PackageManager,
        [Display(Name = "Programming Language")]
        ProgrammingLanguage,
        [Display(Name = "Project Management Tool")]
        ProjectManagementTool,
        [Display(Name = "Search Engine")]
        SearchEngine,
        [Display(Name = "Security Tool")]
        SecurityTool,
        [Display(Name = "Serverless")]
        Serverless,
        [Display(Name = "Styling")]
        Styling,
        [Display(Name = "Testing Tool")]
        TestingTool,
        [Display(Name = "Version Control")]
        VersionControl,
        [Display(Name = "Virtualization")]
        Virtualization,
        [Display(Name = "Web Server")]
        WebServer,
        [Display(Name = "Web Developement")]
        WebDevelopment
    }

    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string Name { get; set; }
    public string? Description { get; set; }
    public string? NameAbbr { get; set; }
    public string? IconUrl { get; set; }
    public string? IconHTML { get; set; }
    public EType Type { get; set; }  // Added Type property
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime ModifiedAt { get; set; } = DateTime.Now;

    public virtual List<Project_TechnologyStack> Project_TechnologyStacks { get; set; }
    public virtual List<Solution_TechnologyStack> Solution_TechnologyStacks { get; set; }
}

