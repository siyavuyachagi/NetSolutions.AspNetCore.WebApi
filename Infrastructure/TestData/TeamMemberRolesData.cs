using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.TestData
{
    public class TeamMemberRolesData
    {
        public static void GenerateProjectTeamMemberRoles(ModelBuilder builder)
        {
            try
            {
                if (Seed.TeamMemberRoles.Any()) return; //Prevent saving duplicates data


                // Seed project roles with Id and Name as string (actual role names)
                var projectRoles = new List<TeamMemberRole>
            {
                new TeamMemberRole { Id = Guid.NewGuid(), Name = "Backend Developer" },
                new TeamMemberRole { Id = Guid.NewGuid(), Name = "Business Analyst" },
                new TeamMemberRole { Id = Guid.NewGuid(), Name = "Developer" },
                new TeamMemberRole { Id = Guid.NewGuid(), Name = "DevOps Engineer" },
                new TeamMemberRole { Id = Guid.NewGuid(), Name = "Frontend Developer" },
                new TeamMemberRole { Id = Guid.NewGuid(), Name = "Full-Stack Developer" },
                new TeamMemberRole { Id = Guid.NewGuid(), Name = "Graphic Designer" },
                new TeamMemberRole { Id = Guid.NewGuid(), Name = "Lead Developer" },
                new TeamMemberRole { Id = Guid.NewGuid(), Name = "Other" },
                new TeamMemberRole { Id = Guid.NewGuid(), Name = "Product Owner" },
                new TeamMemberRole { Id = Guid.NewGuid(), Name = "Project Manager" },
                new TeamMemberRole { Id = Guid.NewGuid(), Name = "QA Engineer" },
                new TeamMemberRole { Id = Guid.NewGuid(), Name = "Scrum Master" },
                new TeamMemberRole { Id = Guid.NewGuid(), Name = "Security Engineer" },
                new TeamMemberRole { Id = Guid.NewGuid(), Name = "Systems Architect" },
                new TeamMemberRole { Id = Guid.NewGuid(), Name = "Technical Writer" },
                new TeamMemberRole { Id = Guid.NewGuid(), Name = "UI/UX Designer" },
            };
                Seed.TeamMemberRoles.AddRange(projectRoles);
                builder.Entity<TeamMemberRole>().HasData(projectRoles);

                Console.WriteLine($"ProjectTeamMemberRoles generated: {Seed.TeamMemberRoles.Count()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error generating PorjectRoles:", ex);
                throw;
            }
        }
    }

}

