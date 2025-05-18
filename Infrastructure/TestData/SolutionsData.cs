using Bogus;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.TestData
{
    public class SolutionsData
    {
        private static readonly Faker _faker = new Faker("en_ZA");
        private static Random _random = new Random();

        public static void GenerateSolutions(ModelBuilder builder)
        {
            try
            {
                var projects = Seed.Projects.ToList();
                foreach (var project in projects)
                {
                    // Base properties for any solution
                    switch (project)
                    {
                        case GraphicDesignProject graphicDesign:
                            {
                                var solution = new Faker<GraphicDesignSolution>("en_ZA")
                                    .RuleFor(s => s.Id, f => Guid.NewGuid())
                                    .RuleFor(s => s.ProjectId, f => graphicDesign.Id)
                                    .RuleFor(s => s.Name, f => graphicDesign.Name)
                                    .RuleFor(s => s.Price, f => f.Random.Decimal(500, 25000))
                                    .RuleFor(s => s.Description, f => graphicDesign.Description)
                                    .RuleFor(s => s.SourceUrl, f => "https://github.com/siyavuyachagi/NetSolutions.Nuxt.Dashboard")
                                    .RuleFor(s => s.PreviewUrl, f => f.Image.PicsumUrl())
                                    .RuleFor(s => s.CreatedAt, f => f.Date.Recent(10))
                                    .RuleFor(s => s.Solution_FileMetadata_Documents, (f, s) =>
                                    {
                                        GenerateSolutionFiles(builder, s, s.ProjectId);
                                        return null;
                                    })
                                    .RuleFor(s => s.Solution_TechnologyStacks, (f, s) =>
                                    {
                                        GenerateSolutionStack(builder, s, s.ProjectId);
                                        return null;
                                    })
                                    .RuleFor(s => s.Solution_FileMetadata_Images, (f, s) =>
                                    {
                                        var files = FilesManager.GetDirFiles("_development_files/graphic-design");
                                        GenerateSolutionImages(builder, s, files);
                                        return null;
                                    })
                                    .RuleFor(s => s.Solution_Reviews, (f, s) =>
                                    {
                                        GenerateSolutionReviews(builder, s);
                                        return null;
                                    })
                                    .RuleFor(s => s.SolutionFeatures, (f, s) =>
                                    {
                                        GenerateSolutionFeatures(builder, s);
                                        return null;
                                    })
                                    .Generate();

                                solution.Project = null;
                                solution.ProjectTeam = null;
                                Seed.Solutions.Add(solution);
                                builder.Entity<GraphicDesignSolution>().HasData(solution);
                            }
                            break;

                        case MobileApplicationProject mobileDev:
                            {
                                var solution = new Faker<MobileApplicationSolution>("en_ZA")
                                .RuleFor(s => s.Id, f => Guid.NewGuid())
                                .RuleFor(s => s.ProjectId, f => mobileDev.Id)
                                .RuleFor(s => s.Name, f => mobileDev.Name)
                                .RuleFor(s => s.Price, f => f.Random.Decimal(500, 25000))
                                .RuleFor(s => s.Description, f => mobileDev.Description)
                                .RuleFor(s => s.SourceUrl, f => $"https://github.com/siyavuyachagi/Node.js")
                                .RuleFor(s => s.PreviewUrl, f => f.Image.PicsumUrl())
                                .RuleFor(s => s.CreatedAt, f => f.Date.Recent(10))
                                .RuleFor(s => s.ProjectTeamId, (f, s) => Seed.ProjectTeams.Where(pt => pt.ProjectId == project.Id).FirstOrDefault().Id)
                                .RuleFor(s => s.Solution_FileMetadata_Documents, (f, s) =>
                                {
                                    GenerateSolutionFiles(builder, s, s.ProjectId);
                                    return null;
                                })
                                .RuleFor(s => s.Solution_TechnologyStacks, (f, s) =>
                                {
                                    GenerateSolutionStack(builder, s, s.ProjectId);
                                    return null;
                                })
                                .RuleFor(s => s.Solution_FileMetadata_Images, (f, s) =>
                                {
                                    var files = FilesManager.GetDirFiles("_development_files/mobile-dev");
                                    GenerateSolutionImages(builder, s, files);
                                    return null;
                                })
                                .RuleFor(s => s.Solution_Reviews, (f, s) =>
                                {
                                    GenerateSolutionReviews(builder, s);
                                    return null;
                                })
                                .RuleFor(s => s.SolutionFeatures, (f, s) =>
                                {
                                    GenerateSolutionFeatures(builder, s);
                                    return null;
                                })
                                .Generate();

                                solution.Project = null;
                                solution.ProjectTeam = null;
                                Seed.Solutions.Add(solution);
                                builder.Entity<MobileApplicationSolution>().HasData(solution);
                            }
                            break;

                        case WebDesignProject webDesign:
                            {
                                var solution = new Faker<WebDesignSolution>("en_ZA")
                                .RuleFor(s => s.Id, f => Guid.NewGuid())
                                .RuleFor(s => s.ProjectId, f => webDesign.Id)
                                .RuleFor(s => s.Name, f => webDesign.Name)
                                .RuleFor(s => s.Price, f => f.Random.Decimal(500, 25000))
                                .RuleFor(s => s.Description, f => webDesign.Description)
                                .RuleFor(s => s.SourceUrl, f => $"https://behance.net/{f.Internet.UserName()}/{f.Hacker.Noun()}")
                                .RuleFor(s => s.PreviewUrl, f => f.Image.PicsumUrl())
                                .RuleFor(s => s.CreatedAt, f => f.Date.Recent(10))
                                .RuleFor(s => s.Solution_FileMetadata_Documents, (f, s) =>
                                {
                                    GenerateSolutionFiles(builder, s, s.ProjectId);
                                    return null;
                                })
                                .RuleFor(s => s.Solution_TechnologyStacks, (f, s) =>
                                {
                                    GenerateSolutionStack(builder, s, s.ProjectId);
                                    return null;
                                })
                                .RuleFor(s => s.Solution_FileMetadata_Images, (f, s) =>
                                {
                                    var files = FilesManager.GetDirFiles("_development_files/web-design");
                                    GenerateSolutionImages(builder, s, files);
                                    return null;
                                })
                                .RuleFor(s => s.Solution_Reviews, (f, s) =>
                                {
                                    GenerateSolutionReviews(builder, s);
                                    return null;
                                })
                                .RuleFor(s => s.SolutionFeatures, (f, s) =>
                                {
                                    GenerateSolutionFeatures(builder, s);
                                    return null;
                                })
                                .Generate();

                                solution.Project = null;
                                solution.ProjectTeam = null;
                                Seed.Solutions.Add(solution);
                                builder.Entity<WebDesignSolution>().HasData(solution);
                            }
                            break;

                        case WebApplicationProject webApp:
                            {
                                var solution = new Faker<WebApplicationSolution>("en_ZA")
                                .RuleFor(s => s.Id, f => Guid.NewGuid())
                                .RuleFor(s => s.ProjectId, f => webApp.Id)
                                .RuleFor(s => s.Name, f => webApp.Name)
                                .RuleFor(s => s.Price, f => f.Random.Decimal(500, 25000))
                                .RuleFor(s => s.Description, f => webApp.Description)
                                .RuleFor(s => s.SourceUrl, f => $"https://github.com/siyavuyachagi/Node.js")
                                .RuleFor(s => s.PreviewUrl, f => f.Image.PicsumUrl())
                                .RuleFor(s => s.CreatedAt, f => f.Date.Recent(10))
                                .RuleFor(s => s.Solution_FileMetadata_Documents, (f, s) =>
                                {
                                    GenerateSolutionFiles(builder, s, s.ProjectId);
                                    return null;
                                })
                                .RuleFor(s => s.Solution_TechnologyStacks, (f, s) =>
                                {
                                    GenerateSolutionStack(builder, s, s.ProjectId);
                                    return null;
                                })
                                .RuleFor(s => s.Solution_FileMetadata_Images, (f, s) =>
                                {
                                    var files = FilesManager.GetDirFiles("_development_files/web-dev");
                                    GenerateSolutionImages(builder, s, files);
                                    return null;
                                })
                                .RuleFor(s => s.Solution_Reviews, (f, s) =>
                                {
                                    GenerateSolutionReviews(builder, s);
                                    return null;
                                })
                                .RuleFor(s => s.SolutionFeatures, (f, s) =>
                                {
                                    GenerateSolutionFeatures(builder, s);
                                    return null;
                                })
                                .Generate();

                                solution.Project = null;
                                solution.ProjectTeam = null;
                                Seed.Solutions.Add(solution);
                                builder.Entity<WebApplicationSolution>().HasData(solution);
                            }
                            break;

                        case UIUXDesignProject uiuxDesign:
                            {
                                var solution = new Faker<UIUXDesignSolution>("en_ZA")
                                .RuleFor(s => s.Id, f => Guid.NewGuid())
                                .RuleFor(s => s.ProjectId, f => uiuxDesign.Id)
                                .RuleFor(s => s.Name, f => uiuxDesign.Name)
                                .RuleFor(s => s.Price, f => f.Random.Decimal(500, 25000))
                                .RuleFor(s => s.Description, f => uiuxDesign.Description)
                                .RuleFor(s => s.SourceUrl, f => $"https://www.figma.com/design/MJ6Mp2CRxl6fBo0AZRqlqw/MyAccountApplication?node-id=0-1&t=anDQ9jHZEPfdv3OD-1")
                                .RuleFor(s => s.PreviewUrl, f => f.Image.PicsumUrl())
                                .RuleFor(s => s.CreatedAt, f => f.Date.Recent(10))
                                .RuleFor(s => s.Solution_FileMetadata_Documents, (f, s) =>
                                {
                                    GenerateSolutionFiles(builder, s, s.ProjectId);
                                    return null;
                                })
                                .RuleFor(s => s.Solution_TechnologyStacks, (f, s) =>
                                {
                                    GenerateSolutionStack(builder, s, s.ProjectId);
                                    return null;
                                })
                                .RuleFor(s => s.Solution_FileMetadata_Images, (f, s) =>
                                {
                                    var files = FilesManager.GetDirFiles("_development_files/uiux-design");
                                    GenerateSolutionImages(builder, s, files);
                                    return null;
                                })
                                .RuleFor(s => s.Solution_Reviews, (f, s) =>
                                {
                                    GenerateSolutionReviews(builder, s);
                                    return null;
                                })
                                .RuleFor(s => s.SolutionFeatures, (f, s) =>
                                {
                                    GenerateSolutionFeatures(builder, s);
                                    return null;
                                })
                                .Generate();

                                solution.Project = null;
                                solution.ProjectTeam = null;
                                Seed.Solutions.Add(solution);
                                builder.Entity<UIUXDesignSolution>().HasData(solution);
                            }
                            break;

                        default:
                            Console.WriteLine($"Project Type: Unknown ({project.GetType().Name})");
                            // No solution generated for unknown project types
                            break;
                    }
                }

                Console.WriteLine($"Solutions generated: {Seed.Solutions.Count()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating Project Solution: {ex.Message}");
                throw;
            }
        }

        private static void GenerateSolutionFiles(ModelBuilder builder, Solution solution, Guid? projectId)
        {
            try
            {
                if (projectId is null)
                {
                    Console.WriteLine($"ProjectId in null, Solution files not generated");
                    return;
                }
                var projectFiles = Seed.Project_FileMetadata_Documents.Where(pts => pts.ProjectId == projectId).ToList() ?? throw new Exception("Cant find ProjectFiles");

                foreach (var file in projectFiles)
                {
                    var solutionFile = new Solution_FileMetadata_Document
                    {
                        Id = Guid.NewGuid(),
                        SolutionId = solution.Id,
                        FileMetadataId = file.FileMetadataId,
                    };
                    Seed.Solution_FileMetadata_Documents.Add(solutionFile);
                    builder.Entity<Solution_FileMetadata_Document>().HasData(solutionFile);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating GenerateSolutionFiles: {ex.Message}");
                throw;
            }
        }

        private static void GenerateSolutionStack(ModelBuilder builder, Solution solution, Guid? projectId)
        {
            try
            {
                if (projectId is null)
                {
                    Console.WriteLine($"ProjectId in null, Solution_TechnologyStack not generated");
                    return;
                }
                var project_TechnologyStacks = Seed.Project_TechnologyStacks.Where(pts => pts.ProjectId == projectId).ToList() ?? throw new Exception("Cant find Project_TechnologyStacks");

                foreach (var project_TechnologyStack in project_TechnologyStacks)
                {
                    var solutionStack = new Solution_TechnologyStack
                    {
                        Id = Guid.NewGuid(),
                        SolutionId = solution.Id,
                        TechnologyStackId = project_TechnologyStack.TechnologyStackId
                    };
                    Seed.Solution_TechnologyStacks.Add(solutionStack);
                    builder.Entity<Solution_TechnologyStack>().HasData(solutionStack);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating Solution_TechnologyStack: {ex.Message}");
                throw;
            }
        }

        private static void GenerateSolutionImages(ModelBuilder builder, Solution solution, List<FilesManager.TestFile> formFiles)
        {
            try
            {
                foreach (var file in formFiles)
                {
                    var fileResource = new FileMetadata
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = file.FileName,
                        ContentType = file.Type,
                        Extension = file.Extension,
                        Size = file.Size,
                        ViewLink = file.VirtualPath,
                        DownloadLink = file.PhysicalPath,
                        StorageProvider = FileMetadata.EStorageProvider.Local

                    };
                    Seed.FileMetadatas.Add(fileResource);
                    builder.Entity<FileMetadata>().HasData(fileResource);

                    var solutionImage = new Solution_FileMetadata_Image
                    {
                        Id = Guid.NewGuid(),
                        SolutionId = solution.Id,
                        FileMetadaId = fileResource.Id,
                    };
                    Seed.Solution_FileMetadata_Images.Add(solutionImage);
                    builder.Entity<Solution_FileMetadata_Image>().HasData(solutionImage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating GenerateSolutionImages: {ex.Message}");
                throw;
            }
        }

        private static void GenerateSolutionReviews(ModelBuilder builder, Solution solution)
        {
            try
            {
                var projects = Seed.Projects.Where(p => p.Id == solution.ProjectId).ToList() ?? throw new Exception($"Projects is null in GenerateSolutionReviews()");

                var reviews = new Faker<Review>("en_ZA")
                    .RuleFor(r => r.Id, f => Guid.NewGuid())
                    .RuleFor(r => r.EvaluatorId, f => f.PickRandom(Seed.Clients).Id)
                    .RuleFor(r => r.Comment, f => f.Lorem.Paragraph())
                    .RuleFor(r => r.Rating, f => f.Random.Int(1, 5))
                    .RuleFor(r => r.CreatedAt, f => f.Date.Past())
                    .Generate(5);
                Seed.Reviews.AddRange(reviews);
                builder.Entity<Review>().HasData(reviews);

                if (reviews is null) throw new Exception($"Reviews is null in GenerateSolutionReviews()");

                foreach (var review in reviews)
                {
                    var solution_review = new Solution_Review
                    {
                        Id = Guid.NewGuid(),
                        SolutionId = solution.Id,
                        ReviewId = review.Id
                    };
                    Seed.Solution_Reviews.Add(solution_review);
                    builder.Entity<Solution_Review>().HasData(solution_review);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating Solution Reviews: {ex.Message}");
                throw;
            }
        }

        private static void GenerateSolutionFeatures(ModelBuilder builder, Solution solution)
        {
            try
            {
                switch (solution)
                {
                    case GraphicDesignSolution:
                        {
                            builder.Entity<SolutionFeature>().HasData(
                                new SolutionFeature { Id = Guid.NewGuid(), SolutionId = solution.Id, Title = "Image Gallery", Description = "Portfolio showcase with high-resolution image display" },
                                new SolutionFeature { Id = Guid.NewGuid(), SolutionId = solution.Id, Title = "File Upload", Description = "Support for uploading and managing design files" },
                                new SolutionFeature { Id = Guid.NewGuid(), SolutionId = solution.Id, Title = "Color Palettes", Description = "Create and save project color schemes" },
                                new SolutionFeature { Id = Guid.NewGuid(), SolutionId = solution.Id, Title = "Version History", Description = "Track revisions of design assets" },
                                new SolutionFeature { Id = Guid.NewGuid(), SolutionId = solution.Id, Title = "Authentication", Description = "Secure client access to projects and assets" }
                            );
                            break;
                        }
                    case WebApplicationSolution:
                        {
                            builder.Entity<SolutionFeature>().HasData(
                                new SolutionFeature { Id = Guid.NewGuid(), SolutionId = solution.Id, Title = "Authentication", Description = "User authentication with multiple providers" },
                                new SolutionFeature { Id = Guid.NewGuid(), SolutionId = solution.Id, Title = "Search", Description = "Full-text search functionality" },
                                new SolutionFeature { Id = Guid.NewGuid(), SolutionId = solution.Id, Title = "Payment Processing", Description = "Integration with payment gateways" },
                                new SolutionFeature { Id = Guid.NewGuid(), SolutionId = solution.Id, Title = "API Integration", Description = "REST API endpoints and third-party integrations" },
                                new SolutionFeature { Id = Guid.NewGuid(), SolutionId = solution.Id, Title = "Content Management", Description = "Dynamic content management system" },
                                new SolutionFeature { Id = Guid.NewGuid(), SolutionId = solution.Id, Title = "Database Migrations", Description = "Automated database schema updates" }
                            );
                            break;
                        }
                    case WebDesignSolution:
                        {
                            builder.Entity<SolutionFeature>().HasData(
                                new SolutionFeature { Id = Guid.NewGuid(), SolutionId = solution.Id, Title = "Responsive Layout", Description = "Mobile-first responsive design" },
                                new SolutionFeature { Id = Guid.NewGuid(), SolutionId = solution.Id, Title = "Accessibility", Description = "WCAG compliant accessible components" },
                                new SolutionFeature { Id = Guid.NewGuid(), SolutionId = solution.Id, Title = "Interactive Prototypes", Description = "Interactive mockups for user testing" },
                                new SolutionFeature { Id = Guid.NewGuid(), SolutionId = solution.Id, Title = "Custom Themes", Description = "Customizable design themes" },
                                new SolutionFeature { Id = Guid.NewGuid(), SolutionId = solution.Id, Title = "Animation", Description = "UI animation and transitions" }
                            );
                            break;
                        }
                    case MobileApplicationSolution:
                        {
                            builder.Entity<SolutionFeature>().HasData(
                                new SolutionFeature { Id = Guid.NewGuid(), SolutionId = solution.Id, Title = "Authentication", Description = "Secure biometric and social authentication" },
                                new SolutionFeature { Id = Guid.NewGuid(), SolutionId = solution.Id, Title = "Offline Support", Description = "Offline data synchronization" },
                                new SolutionFeature { Id = Guid.NewGuid(), SolutionId = solution.Id, Title = "Push Notifications", Description = "Real-time push notification system" },
                                new SolutionFeature { Id = Guid.NewGuid(), SolutionId = solution.Id, Title = "Location Services", Description = "GPS and location-based features" },
                                new SolutionFeature { Id = Guid.NewGuid(), SolutionId = solution.Id, Title = "Device Integration", Description = "Access to device hardware features" },
                                new SolutionFeature { Id = Guid.NewGuid(), SolutionId = solution.Id, Title = "Search", Description = "In-app search functionality" }
                            );
                            break;
                        }
                    case UIUXDesignSolution:
                        {
                            builder.Entity<SolutionFeature>().HasData(
                                new SolutionFeature { Id = Guid.NewGuid(), SolutionId = solution.Id, Title = "User Journey Mapping", Description = "Visual user flow diagrams" },
                                new SolutionFeature { Id = Guid.NewGuid(), SolutionId = solution.Id, Title = "Wireframing", Description = "Low and high-fidelity wireframes" },
                                new SolutionFeature { Id = Guid.NewGuid(), SolutionId = solution.Id, Title = "Usability Testing", Description = "User testing and feedback integration" },
                                new SolutionFeature { Id = Guid.NewGuid(), SolutionId = solution.Id, Title = "Component Library", Description = "Reusable UI component system" },
                                new SolutionFeature { Id = Guid.NewGuid(), SolutionId = solution.Id, Title = "Design System", Description = "Comprehensive design system documentation" }
                            );
                            break;
                        }
                    default:
                        // Log that an unsupported solution type was provided
                        Console.WriteLine($"Warning: No features defined for solution type: {solution}");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating SolutionFeatures: {ex.Message}");
                throw;
            }
        }
    }
}

