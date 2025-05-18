using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.TestData
{
    public class Seed
    {
        #region
        public static List<Administrator> Administrators { get; set; } = new List<Administrator>();
        public static List<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();
        public static List<BusinessService> BusinessServices { get; set; } = new List<BusinessService>();
        public static List<BusinessServicePackage> BusinessServicePackages { get; set; } = new List<BusinessServicePackage>();
        public static List<BusinessServicePackageFeature> BusinessServicePackageFeatures { get; set; } = new List<BusinessServicePackageFeature>();
        public static List<BusinessService_FileMetadata_Images> BusinessService_FileMetadata_Images { get; set; } = new List<BusinessService_FileMetadata_Images>();
        public static List<BusinessService_Testimonial> BusinessService_Testimonials { get; set; } = new List<BusinessService_Testimonial>();
        public static List<Organization> Organizations { get; set; } = new List<Organization>();
        public static List<IdentityRole> IdentityRoles { get; set; } = new List<IdentityRole>();
        public static List<TechnologyStack> TechnologyStacks { get; set; } = new List<TechnologyStack>();
        public static List<HardSkill> HardSkills { get; set; } = new List<HardSkill>();
        public static List<ProjectTeamMember> ProjectTeamMembers { get; set; } = new List<ProjectTeamMember>();
        public static List<ProjectTeamMember_TeamMemberRole> ProjectTeamMember_TeamMemberRoles { get; set; } = new List<ProjectTeamMember_TeamMemberRole>();
        public static List<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
        public static List<SoftSkill> SoftSkills { get; set; } = new List<SoftSkill>();
        public static List<SocialLink> SocialLinks { get; set; } = new List<SocialLink>();
        public static List<Skill> Skills { get; set; } = new List<Skill>();
        public static List<Staff> Staffs { get; set; } = new List<Staff>();
        public static List<GuestUser> GuestUsers { get; set; } = new List<GuestUser>();
        public static List<Solution_FileMetadata_Image> Solution_FileMetadata_Images { get; set; } = new List<Solution_FileMetadata_Image>();
        public static List<Developer> Developers { get; set; } = new List<Developer>();
        public static List<Solution> Solutions { get; set; } = new List<Solution>();
        public static List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public static List<Project_FileMetadata_Document> Project_FileMetadata_Documents { get; set; } = new List<Project_FileMetadata_Document>();
        public static List<Solution_FileMetadata_Document> Solution_FileMetadata_Documents { get; set; } = new List<Solution_FileMetadata_Document>();
        public static List<Designer> Designers { get; set; } = new List<Designer>();
        public static List<Review> Reviews { get; set; } = new List<Review>();
        public static List<Solution_Review> Solution_Reviews { get; set; } = new List<Solution_Review>();
        public static List<FileMetadata> FileMetadatas { get; set; } = new List<FileMetadata>();
        public static List<Profession> Professions { get; set; } = new List<Profession>();
        public static List<ProjectTeam> ProjectTeams { get; set; } = new List<ProjectTeam>();
        public static List<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();
        public static List<TeamMemberRole> TeamMemberRoles { get; set; } = new List<TeamMemberRole>();
        public static List<Project> Projects { get; set; } = new List<Project>();
        public static List<PhysicalAddress> PhysicalAddresses { get; set; } = new List<PhysicalAddress>();
        public static List<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();
        public static List<Project_TechnologyStack> Project_TechnologyStacks { get; set; } = new List<Project_TechnologyStack>();
        public static List<Solution_TechnologyStack> Solution_TechnologyStacks { get; set; } = new List<Solution_TechnologyStack>();
        public static List<Client> Clients { get; set; } = new List<Client>();
        #endregion



        public static void Init(ModelBuilder builder)
        {
            BusinessProfileData.GenerateBusinessProfileData(builder);
            BusinessServicesData.GenerateServices(builder);
            TechnologyStackData.GenerateTechnologyStacks(builder);
            UserRolesData.GenerateUserRoles(builder);
            UserSkillsData.GenerateUserSkills(builder);
            ProfessionsData.GenerateProfessions(builder);
            UsersData.GenerateUsers(builder);
            TeamMemberRolesData.GenerateProjectTeamMemberRoles(builder);
            ProjectsData.GenerateProjects(builder);
            SolutionsData.GenerateSolutions(builder);
            SubscriptionsData.GenerateUserSubscriptions(builder);
        }
    }
}

