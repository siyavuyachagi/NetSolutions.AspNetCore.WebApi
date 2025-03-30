using Microsoft.EntityFrameworkCore;
using NetSolutions.WebApi.Models.Domain;
using NetSolutions.WebApi.Models.Domain;
using Bogus;

namespace NetSolutions.WebApi.TestData;

public class UserSkillsData
{
    public static void GenerateUserSkills(ModelBuilder builder)
    {
        try
        {
            GenerateUserSoftSkills(builder);


            if (!Seed.TechnologyStacks.Any())
            {
                Console.WriteLine("No TechnologyStack, Cannot seed Skills");
                return;
            }

            // Mapping TechnologyStack names to their IDs for reference
            var technologyStacksMap = Seed.TechnologyStacks.Distinct().ToDictionary(ts => ts.Name, ts => ts.Id);
            // Technical skills
            var technicalSkills = new List<TechnicalSkill>
            {
                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "C# Programming")
                    .RuleFor(ts => ts.Description, f => "Proficiency in C# language for application development.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["C#"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["ASP.NET Core"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap[".NET Core"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["Visual Studio"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "JavaScript Development")
                    .RuleFor(ts => ts.Description, f => "Skills in JavaScript for front-end and back-end development.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "Python Scripting")
                    .RuleFor(ts => ts.Description, f => "Writing and executing scripts in Python.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "Java Development")
                    .RuleFor(ts => ts.Description, f => "Building applications using Java.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "TypeScript Programming")
                    .RuleFor(ts => ts.Description, f => "Using TypeScript for large-scale JavaScript applications.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "PHP Development")
                    .RuleFor(ts => ts.Description, f => "Server-side scripting with PHP.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "Go Programming")
                    .RuleFor(ts => ts.Description, f => "Efficient programming with Go.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "Rust Programming")
                    .RuleFor(ts => ts.Description, f => "Systems programming with Rust.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => ".NET Core Development")
                    .RuleFor(ts => ts.Description, f => "Building cross-platform applications with .NET Core.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "ASP.NET Core Development")
                    .RuleFor(ts => ts.Description, f => "Developing web applications with ASP.NET Core.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "Vue.js Development")
                    .RuleFor(ts => ts.Description, f => "Building user interfaces with Vue.js.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "React Development")
                    .RuleFor(ts => ts.Description, f => "Creating interactive UIs with React.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "Angular Development")
                    .RuleFor(ts => ts.Description, f => "Developing web applications with Angular.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "Django Development")
                    .RuleFor(ts => ts.Description, f => "Building web applications with Django.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "Flask Development")
                    .RuleFor(ts => ts.Description, f => "Developing web applications with Flask.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "Spring Boot Development")
                    .RuleFor(ts => ts.Description, f => "Building Java-based applications with Spring Boot.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "jQuery Usage")
                    .RuleFor(ts => ts.Description, f => "Using jQuery for DOM manipulation.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "Lodash Utilities")
                    .RuleFor(ts => ts.Description, f => "Utilizing Lodash for JavaScript utility functions.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "Moment.js Date Handling")
                    .RuleFor(ts => ts.Description, f => "Managing dates with Moment.js.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "Bootstrap Design")
                    .RuleFor(ts => ts.Description, f => "Creating responsive designs with Bootstrap.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["Bootstrap"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "SQL Server Management")
                    .RuleFor(ts => ts.Description, f => "Managing databases with SQL Server.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "MySQL Administration")
                    .RuleFor(ts => ts.Description, f => "Administering MySQL databases.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "PostgreSQL Management")
                    .RuleFor(ts => ts.Description, f => "Managing PostgreSQL databases.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "MongoDB Management")
                    .RuleFor(ts => ts.Description, f => "Managing NoSQL databases with MongoDB.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "SQLite Management")
                    .RuleFor(ts => ts.Description, f => "Managing lightweight databases with SQLite.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "RESTful API Design")
                    .RuleFor(ts => ts.Description, f => "Designing RESTful APIs.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "GraphQL Development")
                    .RuleFor(ts => ts.Description, f => "Building APIs with GraphQL.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "Azure Cloud Services")
                    .RuleFor(ts => ts.Description, f => "Utilizing Azure for cloud services.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "AWS Cloud Services")
                    .RuleFor(ts => ts.Description, f => "Utilizing AWS for cloud services.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "Google Cloud Services")
                    .RuleFor(ts => ts.Description, f => "Utilizing Google Cloud for cloud services.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "Docker Containerization")
                    .RuleFor(ts => ts.Description, f => "Containerizing applications with Docker.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "Kubernetes Orchestration")
                    .RuleFor(ts => ts.Description, f => "Orchestrating containers with Kubernetes.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "GitHub Actions CI/CD")
                    .RuleFor(ts => ts.Description, f => "Automating workflows with GitHub Actions.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "Jenkins CI/CD")
                    .RuleFor(ts => ts.Description, f => "Automating builds with Jenkins.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "Terraform Infrastructure as Code")
                    .RuleFor(ts => ts.Description, f => "Managing infrastructure with Terraform.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "Git Version Control")
                    .RuleFor(ts => ts.Description, f => "Using Git for version control.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "GitHub Code Hosting")
                    .RuleFor(ts => ts.Description, f => "Hosting code on GitHub.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "GitLab Code Hosting")
                    .RuleFor(ts => ts.Description, f => "Hosting code on GitLab.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "Visual Studio Development")
                    .RuleFor(ts => ts.Description, f => "Developing applications with Visual Studio.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["C#"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "VS Code Development")
                    .RuleFor(ts => ts.Description, f => "Developing applications with VS Code.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "Selenium Testing")
                    .RuleFor(ts => ts.Description, f => "Automating browser tests with Selenium.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "xUnit Testing")
                    .RuleFor(ts => ts.Description, f => "Unit testing with xUnit.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["Visual Studio"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "CSS Styling")
                    .RuleFor(ts => ts.Description, f => "Styling web pages with CSS.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TailwindCSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "TailwindCSS Styling")
                    .RuleFor(ts => ts.Description, f => "Styling with TailwindCSS utility classes.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TailwindCSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["Bootstrap"]
                            },
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),

                new Faker<TechnicalSkill>("en_ZA")
                    .RuleFor(ts => ts.Id, f => Guid.NewGuid())
                    .RuleFor(ts => ts.Name, f => "SASS Styling")
                    .RuleFor(ts => ts.Description, f => "Using SASS for CSS preprocessing.")
                    .RuleFor(ts => ts.TechnicalSkill_TechnologyStacks, (f, ts) =>
                    {
                        var technicalSkillTechnologyStacks = new List<TechnicalSkill_TechnologyStack>
                        {
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["JavaScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["CSS"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["TypeScript"]
                            },
                            new TechnicalSkill_TechnologyStack
                            {
                                Id = Guid.NewGuid(),
                                TechnicalSkillId = ts.Id,
                                TechnologyStackId = technologyStacksMap["VS Code"]
                            }
                        };
                        builder.Entity<TechnicalSkill_TechnologyStack>().HasData(technicalSkillTechnologyStacks);
                        return null;
                    })
                    .Generate(),
            };
            Seed.TechnicalSkills.AddRange(technicalSkills);
            Seed.UserSkills.AddRange(technicalSkills);
            builder.Entity<TechnicalSkill>().HasData(technicalSkills);

            Console.WriteLine("GenerateUserSkills Complete");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating UserSkills: {ex.Message}");
            throw;
        }
    }

    private static void GenerateUserSoftSkills(ModelBuilder builder)
    {
        try
        {
            // Soft skills
            var softSkills = new List<SoftSkill>
            {
                new SoftSkill { Id = Guid.NewGuid(), Name = "Communication", Description = "Ability to convey information effectively and efficiently." },
                new SoftSkill { Id = Guid.NewGuid(), Name = "Teamwork", Description = "Working collaboratively with others to achieve a common goal." },
                new SoftSkill { Id = Guid.NewGuid(), Name = "Problem-Solving", Description = "Identifying, analyzing, and solving problems in various situations." },
                new SoftSkill { Id = Guid.NewGuid(), Name = "Adaptability", Description = "Ability to adjust to new conditions and challenges." },
                new SoftSkill { Id = Guid.NewGuid(), Name = "Time Management", Description = "Effectively managing time to complete tasks and meet deadlines." },
                new SoftSkill { Id = Guid.NewGuid(), Name = "Leadership", Description = "Inspiring and influencing others to achieve common goals." },
                new SoftSkill { Id = Guid.NewGuid(), Name = "Critical Thinking", Description = "Analyzing and evaluating information to make reasoned judgments." },
                new SoftSkill { Id = Guid.NewGuid(), Name = "Creativity", Description = "Generating innovative ideas and solutions." },
                new SoftSkill { Id = Guid.NewGuid(), Name = "Project Management", Description = "Effectively manage team projects." }
            };
            Seed.SoftSkills.AddRange(softSkills);
            Seed.UserSkills.AddRange(softSkills);
            builder.Entity<SoftSkill>().HasData(softSkills);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating User SoftSkills: {ex.Message}");
            throw;
        }
    }

}
