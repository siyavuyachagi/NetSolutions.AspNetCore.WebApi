using Bogus;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NetSolutions.WebApi.Models.Domain;
using System.Data;

namespace Infrastructure.TestData
{
    public class ProjectsData
    {
        private static readonly Faker _faker = new Faker("en_ZA");
        private static Random _random = new Random();

        public static void GenerateProjects(ModelBuilder builder)
        {
            try
            {
                var clients = Seed.Clients.ToList() ?? throw new Exception("Cannot find Seed.Clients");

                foreach (var client in clients)
                {
                    //Choose a service
                    var randomService = _faker.PickRandom(Seed.BusinessServices);

                    switch (randomService.Name)
                    {
                        case "Graphic Design":
                            var graphicDesignProject = new Faker<GraphicDesignProject>("en_ZA")
                            .RuleFor(p => p.Id, f => Guid.NewGuid())
                            .RuleFor(p => p.BusinessServiceId, f => randomService.Id)
                            .RuleFor(p => p.Name, f => f.Company.CompanyName())
                            .RuleFor(p => p.Description, f => f.Lorem.Paragraphs(_random.Next(2, 5)))
                            .RuleFor(p => p.ClientId, f => client.Id)
                            .RuleFor(p => p.Budget, f => _faker.Random.Decimal(500, 25000))
                            .RuleFor(p => p.Status, f => f.PickRandom<Project.EStatus>())
                            .RuleFor(p => p.ProjectTeam, (f, p) =>
                            {
                                GenerateDesignProjectTeam(builder, p, Seed.Designers);
                                return null;
                            })
                            .RuleFor(p => p.ProjectMilestones, (f, p) =>
                            {
                                GenerateProjectMilestones(p, builder, randomService);
                                return null;
                            })
                            .RuleFor(p => p.ProjectTasks, (f, p) =>
                            {
                                GenerateProjectTasks(p, builder, randomService);
                                return null;
                            })
                            .Generate(5);
                            Seed.Projects.AddRange(graphicDesignProject);
                            builder.Entity<GraphicDesignProject>().HasData(graphicDesignProject);
                            break;
                        case "Web Development":
                            var webDevelopmentProject = new Faker<WebApplicationProject>("en_ZA")
                            .RuleFor(p => p.Id, f => Guid.NewGuid())
                            .RuleFor(p => p.BusinessServiceId, f => randomService.Id)
                            .RuleFor(p => p.Name, f => f.Company.CompanyName())
                            .RuleFor(p => p.Description, f => _faker.Lorem.Paragraphs(_random.Next(2, 5)))
                            .RuleFor(p => p.ClientId, f => client.Id)
                            .RuleFor(p => p.PreviewUrl, f => f.Internet.Url())
                            .RuleFor(p => p.Budget, f => _faker.Random.Decimal(500, 25000))
                            .RuleFor(p => p.Status, f => f.PickRandom<Project.EStatus>())
                            .RuleFor(p => p.ProjectTeam, (f, p) =>
                            {
                                GenerateDevelopmentProjectTeam(builder, p, Seed.Developers);
                                return null;
                            })
                            .RuleFor(p => p.ProjectMilestones, (f, p) =>
                            {
                                GenerateProjectMilestones(p, builder, randomService);
                                return null;
                            })
                            .RuleFor(p => p.ProjectTasks, (f, p) =>
                            {
                                GenerateProjectTasks(p, builder, randomService);
                                return null;
                            })
                            .Generate(5);
                            Seed.Projects.AddRange(webDevelopmentProject);
                            builder.Entity<WebApplicationProject>().HasData(webDevelopmentProject);
                            break;
                        case "Mobile Development":
                            var mobileDevelopmentProject = new Faker<MobileApplicationProject>("en_ZA")
                            .RuleFor(p => p.Id, f => Guid.NewGuid())
                            .RuleFor(p => p.BusinessServiceId, f => randomService.Id)
                            .RuleFor(p => p.Name, f => f.Company.CompanyName())
                            .RuleFor(p => p.Description, f => _faker.Lorem.Paragraphs(_random.Next(2, 5)))
                            .RuleFor(p => p.ClientId, f => client.Id)
                            .RuleFor(p => p.Budget, f => _faker.Random.Decimal(500, 25000))
                            .RuleFor(p => p.Platform, f => f.PickRandom<MobileApplicationProject.EPlatform>())
                            .RuleFor(p => p.Status, f => f.PickRandom<Project.EStatus>())
                            .RuleFor(p => p.ProjectTeam, (f, p) =>
                            {
                                GenerateDevelopmentProjectTeam(builder, p, Seed.Developers);
                                return null;
                            })
                            .RuleFor(p => p.ProjectMilestones, (f, p) =>
                            {
                                GenerateProjectMilestones(p, builder, randomService);
                                return null;
                            })
                            .RuleFor(p => p.ProjectTasks, (f, p) =>
                            {
                                GenerateProjectTasks(p, builder, randomService);
                                return null;
                            })
                            .Generate(5);
                            Seed.Projects.AddRange(mobileDevelopmentProject);
                            builder.Entity<MobileApplicationProject>().HasData(mobileDevelopmentProject);
                            break;
                        case "UI/UX Design":
                            var uIUXDesignProject = new Faker<UIUXDesignProject>("en_ZA")
                            .RuleFor(p => p.Id, f => Guid.NewGuid())
                            .RuleFor(p => p.BusinessServiceId, f => randomService.Id)
                            .RuleFor(p => p.Name, f => f.Company.CompanyName())
                            .RuleFor(p => p.Description, f => _faker.Lorem.Paragraphs(_random.Next(2, 5)))
                            .RuleFor(p => p.ClientId, f => client.Id)
                            .RuleFor(p => p.Budget, f => _faker.Random.Decimal(500, 25000))
                            .RuleFor(p => p.Status, f => f.PickRandom<Project.EStatus>())
                            .RuleFor(p => p.ProjectTeam, (f, p) =>
                            {
                                GenerateDesignProjectTeam(builder, p, Seed.Designers);
                                return null;
                            })
                            .RuleFor(p => p.ProjectMilestones, (f, p) =>
                            {
                                GenerateProjectMilestones(p, builder, randomService);
                                return null;
                            })
                            .RuleFor(p => p.ProjectTasks, (f, p) =>
                            {
                                GenerateProjectTasks(p, builder, randomService);
                                return null;
                            })
                            .Generate(5);
                            Seed.Projects.AddRange(uIUXDesignProject);
                            builder.Entity<UIUXDesignProject>().HasData(uIUXDesignProject);
                            break;
                        case "Web Design":
                            var webDesignProjects = new Faker<WebDesignProject>("en_ZA")
                            .RuleFor(p => p.Id, f => Guid.NewGuid())
                            .RuleFor(p => p.BusinessServiceId, f => randomService.Id)
                            .RuleFor(p => p.Name, f => f.Company.CompanyName())
                            .RuleFor(p => p.Description, f => _faker.Lorem.Paragraphs(_random.Next(2, 5)))
                            .RuleFor(p => p.ClientId, f => client.Id)
                            .RuleFor(p => p.Budget, f => _faker.Random.Decimal(500, 25000))
                            .RuleFor(p => p.Status, f => f.PickRandom<Project.EStatus>())
                            .RuleFor(p => p.ProjectTeam, (f, p) =>
                            {
                                GenerateDevelopmentProjectTeam(builder, p, Seed.Developers);
                                return null;
                            })
                            .RuleFor(p => p.ProjectMilestones, (f, p) =>
                            {
                                GenerateProjectMilestones(p, builder, randomService);
                                return null;
                            })
                            .RuleFor(p => p.ProjectTasks, (f, p) =>
                            {
                                GenerateProjectTasks(p, builder, randomService);
                                return null;
                            })
                            .Generate(5);
                            Seed.Projects.AddRange(webDesignProjects);
                            builder.Entity<WebDesignProject>().HasData(webDesignProjects);
                            break;
                        default:
                            break;
                    }
                }

                Console.WriteLine($"Projects generated: {Seed.Projects.Count()}");

                Console.WriteLine($"ProjectTasks generated: {Seed.ProjectTasks.Count()}");

                GenerateProjectDocuments(builder);
                GenerateProjectTechnologyStack(builder);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error generating Client Projects: ", ex.Message);
                throw;
            }
        }

        private static void GenerateProjectDocuments(ModelBuilder builder)
        {
            try
            {
                var projects = Seed.Projects.ToList() ?? throw new Exception("Projects is null at GenerateProjectDocuments()");

                foreach (var project in projects)
                {
                    //Get test files
                    var testFiles = FilesManager.GetAllFilesFromTestDir();

                    var randomFiles = testFiles.OrderBy(_ => Guid.NewGuid()).Take(_random.Next(1, 5)) ?? throw new Exception("Error retrieving files in GetAllFilesFromTestDir()");
                    foreach (var file in randomFiles)
                    {
                        var fileResource = new FileMetadata
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = file.FileName,
                            ContentType = file.Type,
                            ViewLink = file.VirtualPath,
                            Extension = file.Extension,
                            Size = file.Size,
                            StorageProvider = FileMetadata.EStorageProvider.Local
                        };
                        Seed.FileMetadatas.Add(fileResource);
                        builder.Entity<FileMetadata>().HasData(fileResource);

                        var projectFile = new Project_FileMetadata_Document
                        {
                            Id = Guid.NewGuid(),
                            ProjectId = project.Id,
                            FileMetadataId = fileResource.Id,
                        };
                        Seed.Project_FileMetadata_Documents.Add(projectFile);
                        builder.Entity<Project_FileMetadata_Document>().HasData(projectFile);
                    }
                }
                Console.WriteLine($"Project Dcouments generated: {Seed.Project_FileMetadata_Documents.Count()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error generating ResourceFiles: ", ex.Message);
                throw;
            }
        }

        private static void GenerateDesignProjectTeam(ModelBuilder builder, Project project, List<Designer> staff)
        {
            try
            {
                if (staff == null | !staff.Any()) throw new InvalidOperationException("staff is null at GenerateDesignProjectTeam()");

                // Generate Team first
                var projectTeam = new Faker<ProjectTeam>("en_ZA")
                    .RuleFor(x => x.Id, f => Guid.NewGuid())
                    .RuleFor(x => x.ProjectId, f => project.Id)
                    .RuleFor(x => x.Name, f => f.Company.CompanyName())
                    .Generate();
                Seed.ProjectTeams.Add(projectTeam);
                builder.Entity<ProjectTeam>().HasData(projectTeam);

                // Ensure we have exactly 4 members, including the ScrumMaster and ProjectManager
                var randomStaffMembers = staff.OrderBy(_ => Guid.NewGuid()).Take(4).ToList();

                // Get Designer TeamMemberRoles
                var projectRoles = GetDesignProjectTeamMemberRoles();

                // Project_Staff junction table
                var projectTeamMembers = new List<ProjectTeamMember>();
                var teamMemberRoles = new List<ProjectTeamMember_TeamMemberRole>();

                // Assign one ScrumMaster
                var scrumMaster = randomStaffMembers.First();
                var scrumMasterTeamMember = new ProjectTeamMember
                {
                    Id = Guid.NewGuid(),
                    ProjectTeamId = projectTeam.Id,
                    MemberId = scrumMaster.Id
                };
                projectTeamMembers.Add(scrumMasterTeamMember);

                // Add the Scrum Master role
                teamMemberRoles.Add(new ProjectTeamMember_TeamMemberRole
                {
                    Id = Guid.NewGuid(),
                    ProjectTeamMemberId = scrumMasterTeamMember.Id,
                    TeamMemberRoleId = Seed.TeamMemberRoles.First(r => r.Name == "Scrum Master").Id
                });

                // Assign one ProjectManager
                var projectManager = randomStaffMembers.Skip(1).First(); // Use Skip(1) to get the second staff member
                var projectManagerTeamMember = new ProjectTeamMember
                {
                    Id = Guid.NewGuid(),
                    ProjectTeamId = projectTeam.Id,
                    MemberId = projectManager.Id
                };
                projectTeamMembers.Add(projectManagerTeamMember);

                // Add the Project Manager role
                teamMemberRoles.Add(new ProjectTeamMember_TeamMemberRole
                {
                    Id = Guid.NewGuid(),
                    ProjectTeamMemberId = projectManagerTeamMember.Id,
                    TeamMemberRoleId = Seed.TeamMemberRoles.First(r => r.Name == "Project Manager").Id
                });

                // Remove ScrumMaster and ProjectManager from the list to assign roles to others
                var remainingStaff = randomStaffMembers.Where(s =>
                    s.Id != scrumMaster.Id && s.Id != projectManager.Id).ToList();

                // Assign relevant roles to the remaining team members
                foreach (var selectedStaff in remainingStaff)
                {
                    // Create team member record
                    var teamMember = new ProjectTeamMember
                    {
                        Id = Guid.NewGuid(),
                        ProjectTeamId = projectTeam.Id,
                        MemberId = selectedStaff.Id
                    };
                    projectTeamMembers.Add(teamMember);

                    // Pick 1st software development role
                    var role = _faker.PickRandom(projectRoles);

                    // Add the primary role
                    teamMemberRoles.Add(new ProjectTeamMember_TeamMemberRole
                    {
                        Id = Guid.NewGuid(),
                        ProjectTeamMemberId = teamMember.Id,
                        TeamMemberRoleId = role.Id
                    });

                    // 50% chance to add a second role
                    if (_random.Next(2) == 0)
                    {
                        var secondRole = _faker.PickRandom(projectRoles);

                        if (secondRole.Id != role.Id) // Ensure different roles by comparing IDs
                        {
                            // Add the secondary role
                            teamMemberRoles.Add(new ProjectTeamMember_TeamMemberRole
                            {
                                Id = Guid.NewGuid(),
                                ProjectTeamMemberId = teamMember.Id,
                                TeamMemberRoleId = secondRole.Id,
                            });
                        }
                    }
                }

                // Add to test data collections AFTER the loop
                Seed.ProjectTeamMembers.AddRange(projectTeamMembers);
                Seed.ProjectTeamMember_TeamMemberRoles.AddRange(teamMemberRoles);

                // Configure the entities for seeding AFTER the loop
                builder.Entity<ProjectTeamMember>().HasData(projectTeamMembers);
                builder.Entity<ProjectTeamMember_TeamMemberRole>().HasData(teamMemberRoles);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating ProjectTeamMembers: {ex.Message}");
                throw;
            }
        }

        private static void GenerateDevelopmentProjectTeam(ModelBuilder builder, Project project, List<Developer> staff)
        {
            try
            {
                if (staff == null | !staff.Any()) throw new InvalidOperationException("staff is null at GenerateDesignProjectTeam()");

                // Generate Team first
                var projectTeam = new Faker<ProjectTeam>("en_ZA")
                    .RuleFor(x => x.Id, f => Guid.NewGuid())
                    .RuleFor(x => x.ProjectId, f => project.Id)
                    .RuleFor(x => x.Name, f => f.Company.CompanyName())
                    .Generate();
                Seed.ProjectTeams.Add(projectTeam);
                builder.Entity<ProjectTeam>().HasData(projectTeam);

                // Ensure we have exactly 4 members, including the ScrumMaster and ProjectManager
                var randomStaffMembers = staff.OrderBy(_ => Guid.NewGuid()).Take(4).ToList();

                // Get Designer TeamMemberRoles
                var projectRoles = GetDevelopmentProjectTeamMemberRoles();

                // Project_Staff junction table
                var projectTeamMembers = new List<ProjectTeamMember>();
                var teamMemberRoles = new List<ProjectTeamMember_TeamMemberRole>();

                // Assign one ScrumMaster
                var scrumMaster = randomStaffMembers.First();
                var scrumMasterTeamMember = new ProjectTeamMember
                {
                    Id = Guid.NewGuid(),
                    ProjectTeamId = projectTeam.Id,
                    MemberId = scrumMaster.Id
                };
                projectTeamMembers.Add(scrumMasterTeamMember);

                // Add the Scrum Master role
                teamMemberRoles.Add(new ProjectTeamMember_TeamMemberRole
                {
                    Id = Guid.NewGuid(),
                    ProjectTeamMemberId = scrumMasterTeamMember.Id,
                    TeamMemberRoleId = Seed.TeamMemberRoles.First(r => r.Name == "Scrum Master").Id
                });

                // Assign one ProjectManager
                var projectManager = randomStaffMembers.Skip(1).First(); // Use Skip(1) to get the second staff member
                var projectManagerTeamMember = new ProjectTeamMember
                {
                    Id = Guid.NewGuid(),
                    ProjectTeamId = projectTeam.Id,
                    MemberId = projectManager.Id
                };
                projectTeamMembers.Add(projectManagerTeamMember);

                // Add the Project Manager role
                teamMemberRoles.Add(new ProjectTeamMember_TeamMemberRole
                {
                    Id = Guid.NewGuid(),
                    ProjectTeamMemberId = projectManagerTeamMember.Id,
                    TeamMemberRoleId = Seed.TeamMemberRoles.First(r => r.Name == "Project Manager").Id
                });

                // Remove ScrumMaster and ProjectManager from the list to assign roles to others
                var remainingStaff = randomStaffMembers.Where(s =>
                    s.Id != scrumMaster.Id && s.Id != projectManager.Id).ToList();

                // Assign relevant roles to the remaining team members
                foreach (var selectedStaff in remainingStaff)
                {
                    // Create team member record
                    var teamMember = new ProjectTeamMember
                    {
                        Id = Guid.NewGuid(),
                        ProjectTeamId = projectTeam.Id,
                        MemberId = selectedStaff.Id
                    };
                    projectTeamMembers.Add(teamMember);

                    // Pick 1st software development role
                    var role = _faker.PickRandom(projectRoles);

                    // Add the primary role
                    teamMemberRoles.Add(new ProjectTeamMember_TeamMemberRole
                    {
                        Id = Guid.NewGuid(),
                        ProjectTeamMemberId = teamMember.Id,
                        TeamMemberRoleId = role.Id
                    });

                    // 50% chance to add a second role
                    if (_random.Next(2) == 0)
                    {
                        var secondRole = _faker.PickRandom(projectRoles);

                        if (secondRole.Id != role.Id) // Ensure different roles by comparing IDs
                        {
                            // Add the secondary role
                            teamMemberRoles.Add(new ProjectTeamMember_TeamMemberRole
                            {
                                Id = Guid.NewGuid(),
                                ProjectTeamMemberId = teamMember.Id,
                                TeamMemberRoleId = secondRole.Id,
                            });
                        }
                    }
                }

                // Add to test data collections AFTER the loop
                Seed.ProjectTeamMembers.AddRange(projectTeamMembers);
                Seed.ProjectTeamMember_TeamMemberRoles.AddRange(teamMemberRoles);

                // Configure the entities for seeding AFTER the loop
                builder.Entity<ProjectTeamMember>().HasData(projectTeamMembers);
                builder.Entity<ProjectTeamMember_TeamMemberRole>().HasData(teamMemberRoles);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating ProjectTeamMembers: {ex.Message}");
                throw;
            }
        }

        private static List<TeamMemberRole> GetDesignProjectTeamMemberRoles()
        {
            try
            {
                var designProjectTeamMemberRoles = Seed.TeamMemberRoles
                    .Where(pr => new[] {
                "Business Analyst",
                "Graphic Designer",
                "Other",
                "Technical Writer",
                "UI/UX Designer" }.Contains(pr.Name))
                    .ToList();

                // If no design roles were found, return all roles as fallback
                if (!designProjectTeamMemberRoles.Any())
                {
                    Console.WriteLine("No specific design roles found. Returning all roles.");
                    return Seed.TeamMemberRoles.ToList();
                }

                return designProjectTeamMemberRoles;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting Design TeamMemberRoles: {ex.Message}");
                throw;
            }
        }

        private static List<TeamMemberRole> GetDevelopmentProjectTeamMemberRoles()
        {
            try
            {
                var developmentProjectTeamMemberRoles = Seed.TeamMemberRoles
                    .Where(pr => new[] {
                    "Backend Developer",
                    "Business Analyst",
                    "DevOps Engineer",
                    "Frontend Developer",
                    "Full-Stack Developer",
                    "Lead Developer",
                    "Other",
                    "QA Engineer",
                    "Security Engineer",
                    "Technical Writer",
                    "UI/UX Designer" }.Contains(pr.Name))
                    .ToList();

                // If no development roles were found, return all roles as fallback
                if (!developmentProjectTeamMemberRoles.Any())
                {
                    Console.WriteLine("No specific development roles found. Returning all roles.");
                    return Seed.TeamMemberRoles.ToList();
                }

                return developmentProjectTeamMemberRoles;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting Developement TeamMemberRoles: ", ex.Message);
                throw;
            }
        }

        private static void GenerateProjectTechnologyStack(ModelBuilder builder)
        {
            try
            {
                var projects = Seed.Projects.ToList() ?? throw new Exception($"Projects is null at GenerateProjectTechnologyStack()");

                foreach (var project in projects)
                {
                    if (project is GraphicDesignProject)
                    {
                        var stackCount = _faker.Random.Number(0, 6);
                        var availableStacks = Seed.TechnologyStacks
                            .Where(ts => new[] { "Adobe Photoshop", "Adobe Illustrator", "Figma", "Sketch", "WordPress", "MySQL" }.Contains(ts.Name))
                            .OrderBy(_ => Guid.NewGuid())
                            .Take(stackCount)
                            .Distinct()
                            .ToList();

                        foreach (var techStack in availableStacks)
                        {
                            var projectTechStack = new Project_TechnologyStack { Id = Guid.NewGuid(), ProjectId = project.Id, TechnologyStackId = techStack.Id };
                            Seed.Project_TechnologyStacks.Add(projectTechStack);
                            builder.Entity<Project_TechnologyStack>().HasData(projectTechStack);
                        }
                    }
                    else if (project is WebApplicationProject)
                    {
                        var stackCount = _faker.Random.Number(0, 6);
                        var availableStacks = Seed.TechnologyStacks
                            .Where(ts => new[] { "HTML5", "CSS3", "JavaScript", "React.js", "Node.js", "MongoDB", "AWS" }.Contains(ts.Name))
                            .OrderBy(_ => Guid.NewGuid())
                            .Take(stackCount)
                            .Distinct()
                            .ToList();

                        foreach (var techStack in availableStacks)
                        {
                            var projectTechStack = new Project_TechnologyStack { Id = Guid.NewGuid(), ProjectId = project.Id, TechnologyStackId = techStack.Id };
                            Seed.Project_TechnologyStacks.Add(projectTechStack);
                            builder.Entity<Project_TechnologyStack>().HasData(projectTechStack);
                        }
                    }
                    else if (project is MobileApplicationProject)
                    {
                        var stackCount = _faker.Random.Number(0, 6);
                        var availableStacks = Seed.TechnologyStacks
                            .Where(ts => new[] { "C#", ".NET MAUI", "Xamarin", "Firebase", "Android Studio", "Swift", "Azure" }.Contains(ts.Name))
                            .OrderBy(_ => Guid.NewGuid())
                            .Take(stackCount)
                            .Distinct()
                            .ToList();

                        foreach (var techStack in availableStacks)
                        {
                            var projectTechStack = new Project_TechnologyStack { Id = Guid.NewGuid(), ProjectId = project.Id, TechnologyStackId = techStack.Id };
                            Seed.Project_TechnologyStacks.Add(projectTechStack);
                            builder.Entity<Project_TechnologyStack>().HasData(projectTechStack);
                        }
                    }
                    else if (project is UIUXDesignProject)
                    {
                        var stackCount = _faker.Random.Number(0, 6);
                        var availableStacks = Seed.TechnologyStacks
                            .Where(ts => new[] { "Figma", "Adobe XD", "Sketch", "InVision", "Zeplin", "UXPin", "Adobe Illustrator" }.Contains(ts.Name))
                            .OrderBy(_ => Guid.NewGuid())
                            .Take(stackCount)
                            .Distinct()
                            .ToList();

                        foreach (var techStack in availableStacks)
                        {
                            var projectTechStack = new Project_TechnologyStack { Id = Guid.NewGuid(), ProjectId = project.Id, TechnologyStackId = techStack.Id };
                            Seed.Project_TechnologyStacks.Add(projectTechStack);
                            builder.Entity<Project_TechnologyStack>().HasData(projectTechStack);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating ProjectTechnologyStack: {ex.Message}");
                throw;
            }
        }

        private static void GenerateProjectMilestones(Project project, ModelBuilder builder, BusinessService service)
        {

            var projectTeam = Seed.ProjectTeams.Where(pt => pt.ProjectId == project.Id).FirstOrDefault();
            var milestones = new List<ProjectMilestone>();

            switch (service.Name)
            {
                case "Graphic Design":
                    switch (project.Status)
                    {
                        case Project.EStatus.NotStarted:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                    .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                    .FirstOrDefault();
                                milestones.Add(new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Initial Concept Meeting", Description = _faker.Lorem.Sentence() });
                            }
                            break;
                        case Project.EStatus.Planning:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                            {
                                new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Completed Documentation", Description = _faker.Lorem.Sentence() },
                                new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Design Brief Approved", Description = _faker.Lorem.Sentence() },
                                new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Mood Board Created", Description = _faker.Lorem.Sentence() },
                                new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Initial Sketches Completed", Description = _faker.Lorem.Sentence() }
                            });
                            }
                            break;
                        case Project.EStatus.InProgress:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Design Drafts Completed", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Color Scheme Finalized", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Typography Selected", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Design Iterations Reviewed", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.OnHold:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Review Pending", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Feedback Incorporation", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Design Adjustments", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Client Review Meeting", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.AwaitingApproval:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Client Feedback Received", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Final Design Tweaks", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Approval Documentation", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Final Client Presentation", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.Testing:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Design Testing Completed", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "User Feedback Collected", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Design Refinements", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Final Testing Report", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.Deployment:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Final Designs Delivered", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Deployment Checklist Completed", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Design Assets Handover", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Post-Deployment Review", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.Maintenance:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Regular Design Updates", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Quarterly Design Review", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Design Maintenance Tasks", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Client Feedback Implementation", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.Completed:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Project Completion Review", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Final Design Documentation", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Client Satisfaction Survey", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Project Closure Report", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.Cancelled:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Cancellation Review", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Feedback Collection", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Documentation Archival", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Final Team Debrief", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        default:
                            break;
                    }
                    break;

                case "Web Development":
                    switch (project.Status)
                    {
                        case Project.EStatus.NotStarted:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.Add(new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Initial Requirements Gathering", Description = _faker.Lorem.Sentence() });
                            }
                            break;
                        case Project.EStatus.Planning:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new() { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Technical Specifications Completed", Description = _faker.Lorem.Sentence() },
                        new() { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Wireframes Created", Description = _faker.Lorem.Sentence() },
                        new() { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Database Schema Designed", Description = _faker.Lorem.Sentence() },
                        new() { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "API Endpoints Defined", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.InProgress:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new() { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Development Milestone 1", Description = _faker.Lorem.Sentence() },
                        new() { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Backend Development Completed", Description = _faker.Lorem.Sentence() },
                        new() { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Frontend Development Completed", Description = _faker.Lorem.Sentence() },
                        new() { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Integration Testing", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.OnHold:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Code Review Pending", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Bug Fixing", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Feature Enhancements", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Client Feedback Incorporation", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.AwaitingApproval:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Client Approval for Deployment", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Final Code Review", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Deployment Checklist", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Pre-Deployment Testing", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.Testing:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Testing Phase Completed", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Bug Reports Resolved", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Performance Testing", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Security Testing", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.Deployment:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new() { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Website Deployed", Description = _faker.Lorem.Sentence() },
                        new() { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Post-Deployment Monitoring", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "User Training Sessions", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Documentation Handover", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.Maintenance:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Regular Updates and Bug Fixes", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Quarterly Maintenance Review", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Security Patches Applied", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Performance Optimizations", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.Completed:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Final Project Review", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Project Closure Documentation", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Client Feedback Collection", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Post-Implementation Review", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.Cancelled:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Cancellation Analysis", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Feedback Collection", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Documentation Archival", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Final Team Debrief", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        default:
                            break;
                    }
                    break;

                case "Mobile Development":
                    switch (project.Status)
                    {
                        case Project.EStatus.NotStarted:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.Add(new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Initial App Concept", Description = _faker.Lorem.Sentence() });
                            }
                            break;
                        case Project.EStatus.Planning:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Wireframes Completed", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "User Stories Defined", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Technical Architecture Planned", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "UI/UX Design Approved", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.InProgress:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Alpha Version Released", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Beta Version Released", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Feature Development Completed", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Bug Fixing and Optimization", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.OnHold:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Beta Testing Feedback", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Design Adjustments", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Feature Re-evaluation", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Client Review Meeting", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.AwaitingApproval:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "App Store Approval", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Final App Demo", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Approval Documentation", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Pre-Launch Marketing", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.Testing:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "User Acceptance Testing Completed", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Bug Reports Resolved", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Performance Testing", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Security Testing", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.Deployment:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "App Released", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "App Store Optimization", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "User Feedback Collection", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Post-Launch Monitoring", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.Maintenance:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Regular App Updates", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Bug Fixes and Patches", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Feature Enhancements", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Performance Optimizations", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.Completed:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Project Closure Meeting", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Final Documentation", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Client Feedback Review", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Post-Project Analysis", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.Cancelled:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Cancellation Report", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Feedback Collection", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Documentation Archival", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Final Team Debrief", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        default:
                            break;
                    }
                    break;

                case "UI/UX Design":
                    switch (project.Status)
                    {
                        case Project.EStatus.NotStarted:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.Add(new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Initial Design Brief", Description = _faker.Lorem.Sentence() });
                            }
                            break;
                        case Project.EStatus.Planning:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "User Personas Created", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "User Journey Mapping", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Information Architecture", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Wireframing", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.InProgress:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Wireframes and Prototypes Completed", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Interactive Prototypes", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Usability Testing", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Design Iterations", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.OnHold:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Design Review Pending", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Feedback Incorporation", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Design Adjustments", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Client Review Meeting", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.AwaitingApproval:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Client Approval for Designs", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Final Design Tweaks", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Approval Documentation", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Final Client Presentation", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.Testing:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Usability Testing Completed", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "User Feedback Collected", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Design Refinements", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Final Testing Report", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.Deployment:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Designs Handed Off to Development", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Deployment Checklist Completed", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Design Assets Handover", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Post-Deployment Review", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.Maintenance:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Regular Design Reviews", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Quarterly Design Audits", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Design Maintenance Tasks", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Client Feedback Implementation", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.Completed:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Final Design Review Meeting", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Project Closure Documentation", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Client Satisfaction Survey", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Post-Project Analysis", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        case Project.EStatus.Cancelled:
                            {
                                var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                milestones.AddRange(new List<ProjectMilestone>
                    {
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Cancellation Feedback Session", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Feedback Collection", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Documentation Archival", Description = _faker.Lorem.Sentence() },
                        new ProjectMilestone { Id = Guid.NewGuid(), ProjectId = project.Id, UserId = teamMember.MemberId, Title = "Final Team Debrief", Description = _faker.Lorem.Sentence() }
                    });
                            }
                            break;
                        default:
                            break;
                    }
                    break;

                default:
                    break;
            }

            builder.Entity<ProjectMilestone>().HasData(milestones);
        }

        private static void GenerateProjectTasks(Project project, ModelBuilder builder, BusinessService service)
        {
            try
            {
                var projectTeam = Seed.ProjectTeams.Where(pt => pt.ProjectId == project.Id).FirstOrDefault() ?? throw new InvalidOperationException("No ProjectTeam found to generate Tasks");
                var tasks = new List<ProjectTask>();

                switch (service.Name)
                {
                    case "Graphic Design":
                        switch (project.Status)
                        {
                            case Project.EStatus.NotStarted:
                                {
                                    var teamMember = Seed.ProjectTeamMembers
                                    .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                    .FirstOrDefault();

                                    tasks.Add(new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(), Name = "Schedule Initial Concept Meeting", Description = _faker.Lorem.Sentence() });
                                    tasks.Add(new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(), Name = "Prepare Meeting Agenda", Description = _faker.Lorem.Sentence() });
                                    tasks.Add(new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(), Name = "Gather Initial Ideas", Description = _faker.Lorem.Sentence() });
                                }
                                break;
                            case Project.EStatus.Planning:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                    .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                    .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                                {
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Create Project Documentation", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Draft Design Brief", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Approve Design Brief", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Create Mood Board", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Complete Initial Sketches", Description = _faker.Lorem.Sentence() }
                                });
                                }
                                break;
                            case Project.EStatus.InProgress:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                    .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                    .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                                {
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Complete Design Drafts", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Finalize Color Scheme", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Select Typography", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Review Design Iterations", Description = _faker.Lorem.Sentence() }
                                });
                                }
                                break;
                            case Project.EStatus.OnHold:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                    .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                    .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                                {
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Review Pending Tasks", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Incorporate Feedback", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Make Design Adjustments", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Schedule Client Review Meeting", Description = _faker.Lorem.Sentence() }
                                });
                                }
                                break;
                            case Project.EStatus.AwaitingApproval:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                        {
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Receive Client Feedback", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Make Final Design Tweaks", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Prepare Approval Documentation", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Present Final Design to Client", Description = _faker.Lorem.Sentence() }
                        });
                                }
                                break;
                            case Project.EStatus.Testing:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                        {
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Complete Design Testing", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Collect User Feedback", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Make Design Refinements", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Prepare Final Testing Report", Description = _faker.Lorem.Sentence() }
                        });
                                }
                                break;
                            case Project.EStatus.Deployment:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                        {
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Deliver Final Designs", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Complete Deployment Checklist", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Handover Design Assets", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Post-Deployment Review", Description = _faker.Lorem.Sentence() }
                        });
                                }
                                break;
                            case Project.EStatus.Maintenance:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                        {
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Perform Regular Design Updates", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Quarterly Design Review", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Complete Design Maintenance Tasks", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Implement Client Feedback", Description = _faker.Lorem.Sentence() }
                        });
                                }
                                break;
                            case Project.EStatus.Completed:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                        {
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Project Completion Review", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Prepare Final Design Documentation", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Administer Client Satisfaction Survey", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Prepare Project Closure Report", Description = _faker.Lorem.Sentence() }
                        });
                                }
                                break;
                            case Project.EStatus.Cancelled:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                        {
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Cancellation Review", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Collect Feedback", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Archive Documentation", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Final Team Debrief", Description = _faker.Lorem.Sentence() }
                        });
                                }
                                break;
                            default:
                                break;
                        }
                        break;

                    case "Web Development":
                        switch (project.Status)
                        {
                            case Project.EStatus.NotStarted:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.Add(new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(), Name = "Gather Initial Requirements", Description = _faker.Lorem.Sentence() });
                                }
                                break;
                            case Project.EStatus.Planning:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                        {
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Complete Technical Specifications", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Create Wireframes", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Design Database Schema", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Define API Endpoints", Description = _faker.Lorem.Sentence() }
                        });
                                }
                                break;
                            case Project.EStatus.InProgress:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                        {
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Complete Development Milestone 1", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Complete Backend Development", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Complete Frontend Development", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Integration Testing", Description = _faker.Lorem.Sentence() }
                        });
                                }
                                break;
                            case Project.EStatus.OnHold:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                        {
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Review Pending Code", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Fix Bugs", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Enhance Features", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Incorporate Client Feedback", Description = _faker.Lorem.Sentence() }
                        });
                                }
                                break;
                            case Project.EStatus.AwaitingApproval:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                        {
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Obtain Client Approval for Deployment", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Final Code Review", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Complete Deployment Checklist", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Pre-Deployment Testing", Description = _faker.Lorem.Sentence() }
                        });
                                }
                                break;
                            case Project.EStatus.Testing:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                        {
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Complete Testing Phase", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Resolve Bug Reports", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Performance Testing", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Security Testing", Description = _faker.Lorem.Sentence() }
                        });
                                }
                                break;
                            case Project.EStatus.Deployment:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                        {
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Deploy Website", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Monitor Post-Deployment", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct User Training Sessions", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Handover Documentation", Description = _faker.Lorem.Sentence() }
                        });
                                }
                                break;
                            case Project.EStatus.Maintenance:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                        {
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Perform Regular Updates and Bug Fixes", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Quarterly Maintenance Review", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Apply Security Patches", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Optimize Performance", Description = _faker.Lorem.Sentence() }
                        });
                                }
                                break;
                            case Project.EStatus.Completed:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                        {
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Final Project Review", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Prepare Project Closure Documentation", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Collect Client Feedback", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Post-Implementation Review", Description = _faker.Lorem.Sentence() }
                        });
                                }
                                break;
                            case Project.EStatus.Cancelled:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                        {
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Cancellation Analysis", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Collect Feedback", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Archive Documentation", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Final Team Debrief", Description = _faker.Lorem.Sentence() }
                        });
                                }
                                break;
                            default:
                                break;
                        }
                        break;

                    case "Mobile Development":
                        switch (project.Status)
                        {
                            case Project.EStatus.NotStarted:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                    .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                    .FirstOrDefault();
                                    tasks.Add(new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(), Name = "Conceptualize Initial App Idea", Description = _faker.Lorem.Sentence() });
                                }
                                break;
                            case Project.EStatus.Planning:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                    .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                    .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                                {
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Complete Wireframes", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Define User Stories", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Plan Technical Architecture", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Approve UI/UX Design", Description = _faker.Lorem.Sentence() }
                                });
                                }
                                break;
                            case Project.EStatus.InProgress:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                        {
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Release Alpha Version", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Release Beta Version", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Complete Feature Development", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Optimize and Fix Bugs", Description = _faker.Lorem.Sentence() }
                        });
                                }
                                break;
                            case Project.EStatus.OnHold:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                        {
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Review Beta Testing Feedback", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Make Design Adjustments", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Re-evaluate Features", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Schedule Client Review Meeting", Description = _faker.Lorem.Sentence() }
                        });
                                }
                                break;
                            case Project.EStatus.AwaitingApproval:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                        {
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Obtain App Store Approval", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Final App Demo", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Prepare Approval Documentation", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Initiate Pre-Launch Marketing", Description = _faker.Lorem.Sentence() }
                        });
                                }
                                break;
                            case Project.EStatus.Testing:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                        {
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Complete User Acceptance Testing", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Resolve Bug Reports", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Performance Testing", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Security Testing", Description = _faker.Lorem.Sentence() }
                        });
                                }
                                break;
                            case Project.EStatus.Deployment:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                        {
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Release App", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Optimize App Store Listing", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Collect User Feedback", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Monitor Post-Launch", Description = _faker.Lorem.Sentence() }
                        });
                                }
                                break;
                            case Project.EStatus.Maintenance:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                        {
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Perform Regular App Updates", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Apply Bug Fixes and Patches", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Enhance Features", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Optimize Performance", Description = _faker.Lorem.Sentence() }
                        });
                                }
                                break;
                            case Project.EStatus.Completed:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                        {
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Project Closure Meeting", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Prepare Final Documentation", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Review Client Feedback", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Post-Project Analysis", Description = _faker.Lorem.Sentence() }
                        });
                                }
                                break;
                            case Project.EStatus.Cancelled:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                        {
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Prepare Cancellation Report", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Collect Feedback", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Archive Documentation", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Final Team Debrief", Description = _faker.Lorem.Sentence() }
                        });
                                }
                                break;
                            default:
                                break;
                        }
                        break;

                    case "UI/UX Design":
                        switch (project.Status)
                        {
                            case Project.EStatus.NotStarted:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.Add(new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(), Name = "Create Initial Design Brief", Description = _faker.Lorem.Sentence() });
                                }
                                break;
                            case Project.EStatus.Planning:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                        {
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Create User Personas", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Map User Journeys", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Develop Information Architecture", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Create Wireframes", Description = _faker.Lorem.Sentence() }
                        });
                                }
                                break;
                            case Project.EStatus.InProgress:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                        {
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Complete Wireframes and Prototypes", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Develop Interactive Prototypes", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Usability Testing", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Iterate Designs Based on Feedback", Description = _faker.Lorem.Sentence() }
                        });
                                }
                                break;
                            case Project.EStatus.OnHold:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                        {
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Review Pending Design Tasks", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Incorporate Feedback", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Make Design Adjustments", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Schedule Client Review Meeting", Description = _faker.Lorem.Sentence() }
                        });
                                }
                                break;
                            case Project.EStatus.AwaitingApproval:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                        {
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Obtain Client Approval for Designs", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Make Final Design Tweaks", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Prepare Approval Documentation", Description = _faker.Lorem.Sentence() },
                            new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Present Final Design to Client", Description = _faker.Lorem.Sentence() }
                        });
                                }
                                break;
                            case Project.EStatus.Testing:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                    .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                    .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                                {
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Complete Usability Testing", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Collect User Feedback", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Make Design Refinements", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Prepare Final Testing Report", Description = _faker.Lorem.Sentence() }
                                });
                                }
                                break;
                            case Project.EStatus.Deployment:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                    .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                    .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                                {
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Handover Designs to Development", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Complete Deployment Checklist", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Handover Design Assets", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Post-Deployment Review", Description = _faker.Lorem.Sentence() }
                                });
                                }
                                break;
                            case Project.EStatus.Maintenance:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                    .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                    .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                                {
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Regular Design Reviews", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Quarterly Design Audits", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Complete Design Maintenance Tasks", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Implement Client Feedback", Description = _faker.Lorem.Sentence() }
                                });
                                }
                                break;
                            case Project.EStatus.Completed:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                    .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                    .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                                {
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Final Design Review Meeting", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Prepare Project Closure Documentation", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Administer Client Satisfaction Survey", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Post-Project Analysis", Description = _faker.Lorem.Sentence() }
                                });
                                }
                                break;
                            case Project.EStatus.Cancelled:
                                {

                                    var teamMember = Seed.ProjectTeamMembers
                                    .Where(pt => pt.ProjectTeamId == projectTeam.Id)
                                    .FirstOrDefault();
                                    tasks.AddRange(new List<ProjectTask>
                                {
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Cancellation Feedback Session", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Collect Feedback", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Archive Documentation", Description = _faker.Lorem.Sentence() },
                                    new ProjectTask { Id = Guid.NewGuid(), ProjectId = project.Id, Priority = _faker.PickRandom<ProjectTask.EPriority>(), Status = _faker.PickRandom<ProjectTask.EStatus>(), DueAt = _faker.Date.Future(),Name = "Conduct Final Team Debrief", Description = _faker.Lorem.Sentence() }
                                });
                                }
                                break;
                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }

                Seed.ProjectTasks.AddRange(tasks);
                builder.Entity<ProjectTask>().HasData(tasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating ProjectTasks: {ex.Message}");
                throw;
            }
        }

    }
}

