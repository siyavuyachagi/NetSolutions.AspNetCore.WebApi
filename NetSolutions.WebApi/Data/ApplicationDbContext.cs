using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetSolutions.WebApi.Models.Domain;
using NetSolutions.WebApi.TestData;

namespace NetSolutions.WebApi.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Staff_Skill>()
        .HasOne(x => x.UserSkill)
        .WithMany(x => x.Staff_Skills)
        .HasForeignKey(x => x.UserSkillId)
        .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

        builder.Entity<Project_FileMetadata_Document>()
            .HasOne(d => d.Project)
            .WithMany(p => p.Documents)
            .HasForeignKey(d => d.ProjectId);

        builder.Entity<Project_FileMetadata_Document>()
            .HasOne(d => d.FileMetadata)
            .WithMany() // Assuming FileMetadata isn't aware of the relationship
            .HasForeignKey(d => d.FileMetadataId);

        builder.Entity<ProjectMilestone>()
        .HasOne(x => x.Project)
        .WithMany(x => x.ProjectMilestones)
        .HasForeignKey(x => x.ProjectId)
        .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<TeamMember>()
        .HasOne(x => x.ProjectTeam)
        .WithMany(x => x.TeamMembers)
        .HasForeignKey(x => x.ProjectTeamId)
        .OnDelete(DeleteBehavior.Restrict);

        //Seed.Init(builder);
    }



    public DbSet<BusinessService> BusinessServices { get; set; }
    public DbSet<BusinessServicePackage> BusinessServicePackages { get; set; }

    public DbSet<Certification> Certifications { get; set; }
    public DbSet<Client> Clients { get; set; }

    public DbSet<FileMetadata> FileMetadatas { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }

    public DbSet<GraphicDesignProject> GraphicDesignProjects { get; set; }
    public DbSet<GraphicDesignSolution> GraphicDesignSolutions { get; set; }

    public DbSet<MobileApplicationProject> MobileApplicationProjects { get; set; }
    public DbSet<MobileApplicationSolution> MobileApplicationSolutions { get; set; }

    public DbSet<Organization> Organizations { get; set; }

    public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
    public DbSet<ProjectMilestone> ProjectMilestones { get; set; }
    public DbSet<Profession> Professions { get; set; }
    public DbSet<Project> Projects { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<Solution> Solutions { get; set; }
    public DbSet<SolutionFeature> SolutionFeatures { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Staff> Staff { get; set; }
    public DbSet<SystemLogEntry> SystemLogEntries { get; set; }

    public DbSet<TeamMember> TeamMembers { get; set; }
    public DbSet<TeamMemberRole> TeamMemberRoles { get; set; }
    public DbSet<TechnicalSkill> TechnicalSkills { get; set; }
    public DbSet<TechnologyStack> TechnologyStacks { get; set; }
    public DbSet<Testimonial> Testimonials { get; set; }

    public DbSet<UIUXDesignProject> UIUXDesignProjects { get; set; }
    public DbSet<UIUXDesignSolution> UIUXDesignSolutions { get; set; }
    public DbSet<User_Solution_PaymentTransaction> User_Solution_PaymentTransactions { get; set; }
    public DbSet<UserActivity> UserActivities { get; set; }
    public DbSet<UserSkill> UserSkills { get; set; }

    public DbSet<WebApplicationProject> WebApplicationProjects { get; set; }
    public DbSet<WebApplicationSolution> WebApplicationSolutions { get; set; }
    public DbSet<WebDesignProject> WebDesignProjects { get; set; }
    public DbSet<WebDesignSolution> WebDesignSolutions { get; set; }
}
