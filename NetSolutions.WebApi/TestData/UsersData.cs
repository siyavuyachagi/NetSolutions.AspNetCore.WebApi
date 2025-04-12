using Bogus;
using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetSolutions.Models;
using NetSolutions.WebApi.Models.Domain;
using NetSolutions.Services;
using System.ComponentModel.Design;
using NetSolutions.WebApi.TestData;

namespace NetSolutions.WebApi.TestData;

public class UsersData
{
    private static readonly Random _random = new Random();

    /// <summary>
    /// Initial method to seed Users, including Clients with Projects
    /// </summary>
    /// <param name="builder"></param>
    public static void GenerateUsers(ModelBuilder builder)
    {
        try
        {
            GenerateAdministrators(builder);
            GenerateStaff(builder);
            GenerateDevelopers(builder);
            GenerateDesigners(builder);
            GenerateClients(builder);
            GenerateOtherUsers(builder);

            Console.WriteLine("GenerateUsers Complete");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing GenerateUsers() :{ex.Message}");
            throw;
        }
    }

    private static void GenerateAdministrators(ModelBuilder builder)
    {
        try
        {
            var professionsMap = Seed.Professions.ToDictionary(p => p.Name, p => p) ?? throw new Exception("Error generating Administrators, Professions null");
            var rolesMap = Seed.IdentityRoles.ToDictionary(r => r.Name, r => r) ?? throw new Exception("Error generating Administrators, Roles are null");


            //✅ Seed Administrator
            var administrators = new Faker<Administrator>("en_ZA")
            .RuleFor(a => a.Id, f => Guid.NewGuid().ToString())
            .RuleFor(a => a.Gender, f => "Male")
            .RuleFor(a => a.LastName, f => "Chagi")
            .RuleFor(a => a.FirstName, f => "Siyavuya")
            .RuleFor(a => a.StaffNumber, (f, a) => $"{a.LastName.Substring(0, 1)}{a.FirstName.Substring(0, 1)}{f.Person.DateOfBirth.ToString("yy/MM/dd").Replace("/", "")}")
            .RuleFor(a => a.Email, f => "syavuya08@gmail.com")
            .RuleFor(a => a.NormalizedEmail, (f, a) => a.Email.ToUpper())
            .RuleFor(a => a.UserName, (f, a) => a.Email)
            .RuleFor(a => a.NormalizedUserName, (f, a) => a.Email.ToUpper())
            .RuleFor(a => a.EmailConfirmed, f => true)
            .RuleFor(a => a.PhoneNumber, f => f.Phone.PhoneNumber("+27 ## ### ####"))
            .RuleFor(a => a.PasswordHash, (f, a) => new PasswordHasher<Administrator>().HashPassword(a, "Administrator@123"))
            .RuleFor(s => s.Avatar, (f, s) =>
                $"https://randomuser.me/api/portraits/{(s.Gender == "Male" ? "men" : "women")}/{f.Random.Int(1, 100)}.jpg")
            .RuleFor(s => s.ProfessionId, f => professionsMap["Software Developer"].Id)
            .RuleFor(a => a.Bio, f => f.Lorem.Paragraphs(_random.Next(1, 3)))
            .RuleFor(a => a.Skills, (f, a) =>
            {
                var softSkills = Seed.SoftSkills.OrderBy(_ => Guid.NewGuid()).Take(_random.Next(1, Seed.SoftSkills.Count + 1)).ToList();
                foreach (var skill in softSkills)
                {
                    builder.Entity<Staff_Skill>().HasData(
                        new Staff_Skill { Id = Guid.NewGuid(), StaffId = a.Id, UserSkillId = skill.Id, YearsOfExperience = _random.Next(1, 6) }
                    );
                }

                var technicalSkillsMap = Seed.TechnicalSkills.ToDictionary(ts => ts.Name, ts => ts);
                builder.Entity<Staff_Skill>().HasData(
                    new Staff_Skill { Id = Guid.NewGuid(), StaffId = a.Id, UserSkillId = technicalSkillsMap["C# Programming"].Id, YearsOfExperience = _random.Next(1, 6) },
                    new Staff_Skill { Id = Guid.NewGuid(), StaffId = a.Id, UserSkillId = technicalSkillsMap["JavaScript Development"].Id, YearsOfExperience = _random.Next(1, 6) },
                    new Staff_Skill { Id = Guid.NewGuid(), StaffId = a.Id, UserSkillId = technicalSkillsMap["JavaScript Development"].Id, YearsOfExperience = _random.Next(1, 6) },
                    new Staff_Skill { Id = Guid.NewGuid(), StaffId = a.Id, UserSkillId = technicalSkillsMap[".NET Core Development"].Id, YearsOfExperience = _random.Next(1, 6) },
                    new Staff_Skill { Id = Guid.NewGuid(), StaffId = a.Id, UserSkillId = technicalSkillsMap["ASP.NET Core Development"].Id, YearsOfExperience = _random.Next(1, 6) },
                    new Staff_Skill { Id = Guid.NewGuid(), StaffId = a.Id, UserSkillId = technicalSkillsMap["Vue.js Development"].Id, YearsOfExperience = _random.Next(1, 6) },
                    new Staff_Skill { Id = Guid.NewGuid(), StaffId = a.Id, UserSkillId = technicalSkillsMap["jQuery Usage"].Id, YearsOfExperience = _random.Next(1, 6) },
                    new Staff_Skill { Id = Guid.NewGuid(), StaffId = a.Id, UserSkillId = technicalSkillsMap["Bootstrap Design"].Id, YearsOfExperience = _random.Next(1, 6) },
                    new Staff_Skill { Id = Guid.NewGuid(), StaffId = a.Id, UserSkillId = technicalSkillsMap["RESTful API Design"].Id, YearsOfExperience = _random.Next(1, 6) },
                    new Staff_Skill { Id = Guid.NewGuid(), StaffId = a.Id, UserSkillId = technicalSkillsMap["Azure Cloud Services"].Id, YearsOfExperience = _random.Next(1, 6) },
                    new Staff_Skill { Id = Guid.NewGuid(), StaffId = a.Id, UserSkillId = technicalSkillsMap["Git Version Control"].Id, YearsOfExperience = _random.Next(1, 6) },
                    new Staff_Skill { Id = Guid.NewGuid(), StaffId = a.Id, UserSkillId = technicalSkillsMap["Visual Studio Development"].Id, YearsOfExperience = _random.Next(1, 6) },
                    new Staff_Skill { Id = Guid.NewGuid(), StaffId = a.Id, UserSkillId = technicalSkillsMap["VS Code Development"].Id, YearsOfExperience = _random.Next(1, 6) },
                    new Staff_Skill { Id = Guid.NewGuid(), StaffId = a.Id, UserSkillId = technicalSkillsMap["Selenium Testing"].Id, YearsOfExperience = _random.Next(1, 6) },
                    new Staff_Skill { Id = Guid.NewGuid(), StaffId = a.Id, UserSkillId = technicalSkillsMap["xUnit Testing"].Id, YearsOfExperience = _random.Next(1, 6) },
                    new Staff_Skill { Id = Guid.NewGuid(), StaffId = a.Id, UserSkillId = technicalSkillsMap["TailwindCSS Styling"].Id, YearsOfExperience = _random.Next(1, 6) }
                );

                return null;
            })
            .Generate(1)
            .ToList();

            administrators.ForEach(admin =>
            {
                builder.Entity<IdentityUserRole<string>>().HasData(
                    new IdentityUserRole<string> { UserId = admin.Id, RoleId = rolesMap[nameof(Staff)].Id },
                    new IdentityUserRole<string> { UserId = admin.Id, RoleId = rolesMap[nameof(Administrator)].Id },
                    new IdentityUserRole<string> { UserId = admin.Id, RoleId = rolesMap[nameof(Developer)].Id }
                );
                Seed.ApplicationUsers.Add(admin);
                Seed.Administrators.Add(admin);
                builder.Entity<Administrator>().HasData(admin);
            });

            Console.WriteLine($"Admins generated: {Seed.Administrators.Count()}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error generating Administrators: ", ex.Message);
            throw;
        }
    }

    private static void GenerateStaff(ModelBuilder builder)
    {
        try
        {
            var rolesMap = Seed.IdentityRoles.ToDictionary(r => r.Name, r => r) ?? throw new Exception("Error generating Staff, Roles are null");

            //✅ Seed Staff
            var staff = new Faker<Staff>("en_ZA")
            .RuleFor(a => a.Id, f => Guid.NewGuid().ToString())
            .RuleFor(u => u.Gender, f => f.Person.Gender.ToString())
            .RuleFor(u => u.LastName, (f, u) => f.Name.LastName(u.Gender == nameof(Bogus.DataSets.Name.Gender.Male) ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female))
            .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName(u.Gender == nameof(Bogus.DataSets.Name.Gender.Male) ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female))
            .RuleFor(a => a.StaffNumber, (f, a) => $"{a.LastName.Substring(0, 1)}{a.FirstName.Substring(0, 1)}{f.Person.DateOfBirth.ToString("yy/MM/dd").Replace("/", "")}")
            .RuleFor(s => s.Email, (f, s) => f.Internet.Email(s.FirstName, s.LastName))
            .RuleFor(a => a.NormalizedEmail, (f, a) => a.Email.ToUpper())
            .RuleFor(a => a.UserName, (f, a) => a.Email)
            .RuleFor(a => a.NormalizedUserName, (f, a) => a.Email.ToUpper())
            .RuleFor(a => a.EmailConfirmed, f => true)
            .RuleFor(a => a.PhoneNumber, f => f.Phone.PhoneNumber("+27 ## ### ####"))
            .RuleFor(a => a.PasswordHash, (f, a) => new PasswordHasher<Staff>().HashPassword(a, "Staff@123"))
            .RuleFor(s => s.Avatar, (f, s) =>
                $"https://randomuser.me/api/portraits/{(s.Gender == "Male" ? "men" : "women")}/{f.Random.Int(1,100)}.jpg")
            .RuleFor(s => s.ProfessionId, f => f.PickRandom(Seed.Professions).Id)
            .RuleFor(s => s.Skills, (f, s) =>
            {
                var softSkills = Seed.SoftSkills.OrderBy(_ => Guid.NewGuid()).Take(_random.Next(1, Seed.SoftSkills.Count + 1)).ToList();
                foreach (var skill in softSkills)
                {
                    builder.Entity<Staff_Skill>().HasData(
                        new Staff_Skill { Id = Guid.NewGuid(), StaffId = s.Id, UserSkillId = skill.Id, YearsOfExperience = _random.Next(2, 6) }
                     );
                }
                return null;
            })
            .RuleFor(a => a.Bio, f => f.Lorem.Paragraphs(_random.Next(1, 3)))
            .Generate(5)
            .ToList();

            staff.ForEach(staff =>
            {
                builder.Entity<IdentityUserRole<string>>().HasData(
                    new IdentityUserRole<string> { UserId = staff.Id, RoleId = rolesMap[nameof(Staff)].Id }
                );
                Seed.ApplicationUsers.Add(staff);
                Seed.Staffs.Add(staff);
                builder.Entity<Staff>().HasData(staff);
            });

            Console.WriteLine($"Staff generated: {Seed.Staffs.Count()}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error generating Staff: ", ex.Message);
            throw;
        }
    }

    private static void GenerateDevelopers(ModelBuilder builder)
    {
        try
        {
            var professionsMap = Seed.Professions.ToDictionary(p => p.Name, p => p) ?? throw new Exception("Error generating Administrators, Professions null");
            Dictionary<string?, IdentityRole> rolesMap = Seed.IdentityRoles.ToDictionary(r => r.Name, r => r) ?? throw new Exception("Error generating Staff, Roles are null");

            //✅ Seed Developers
            var developers = new Faker<Developer>("en_ZA")
            .RuleFor(s => s.Id, f => Guid.NewGuid().ToString())
            .RuleFor(u => u.Gender, f => f.Person.Gender.ToString())
            .RuleFor(u => u.LastName, (f, u) => f.Name.LastName(u.Gender == nameof(Bogus.DataSets.Name.Gender.Male) ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female))
            .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName(u.Gender == nameof(Bogus.DataSets.Name.Gender.Male) ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female))
            .RuleFor(s => s.StaffNumber, (f, s) => $"{s.LastName.Substring(0, 1)}{s.FirstName.Substring(0, 1)}{f.Person.DateOfBirth.ToString("yy/MM/dd").Replace("/", "")}")
            .RuleFor(s => s.Email, (f, s) => f.Internet.Email(s.FirstName, s.LastName))
            .RuleFor(s => s.NormalizedEmail, (f, s) => s.Email.ToUpper())
            .RuleFor(s => s.UserName, (f, s) => s.Email)
            .RuleFor(s => s.NormalizedUserName, (f, s) => s.Email.ToUpper())
            .RuleFor(s => s.EmailConfirmed, f => true)
            .RuleFor(s => s.PhoneNumber, f => f.Phone.PhoneNumber("+27 ## ### ####"))
            .RuleFor(s => s.PasswordHash, (f, s) => new PasswordHasher<Developer>().HashPassword(s, "Developer@123"))
            .RuleFor(s => s.Avatar, (f, s) =>
                $"https://randomuser.me/api/portraits/{(s.Gender == "Male" ? "men" : "women")}/{f.Random.Int(1, 100)}.jpg")
            .RuleFor(s => s.ProfessionId, f =>
            {
                return f.PickRandom(professionsMap["Software Developer"].Id, professionsMap["Frontend Developer"].Id);
            })
            .RuleFor(a => a.Skills, (f, a) =>
            {
                var softSkills = Seed.SoftSkills.OrderBy(_ => Guid.NewGuid()).Take(_random.Next(1, Seed.SoftSkills.Count + 1)).ToList();
                foreach (var skill in softSkills)
                {
                    builder.Entity<Staff_Skill>().HasData(
                        new Staff_Skill { Id = Guid.NewGuid(), StaffId = a.Id, UserSkillId = skill.Id, YearsOfExperience = _random.Next(1, 6) }
                    );
                }

                var technicalSkills = Seed.TechnicalSkills.OrderBy(_ => Guid.NewGuid()).Take(_random.Next(1, Seed.TechnicalSkills.Count + 1)).ToList();
                foreach (var skill in technicalSkills)
                {
                    builder.Entity<Staff_Skill>().HasData(
                        new Staff_Skill { Id = Guid.NewGuid(), StaffId = a.Id, UserSkillId = skill.Id, YearsOfExperience = _random.Next(4, 10) }
                    );
                }
                return null;
            })
            .RuleFor(a => a.Bio, f => f.Lorem.Paragraphs(_random.Next(1, 3)))
            .Generate(5)
            .ToList();

            if (developers == null | !developers.Any()) throw new InvalidOperationException("Developer is null at GenerateDevelopers()");

            developers.ForEach(developer =>
            {
                builder.Entity<IdentityUserRole<string>>().HasData(
                    new IdentityUserRole<string> { UserId = developer.Id, RoleId = rolesMap[nameof(Staff)].Id },
                    new IdentityUserRole<string> { UserId = developer.Id, RoleId = rolesMap[nameof(Developer)].Id }
                );
                Seed.ApplicationUsers.Add(developer);
                Seed.Developers.Add(developer);
                builder.Entity<Developer>().HasData(developer);
            });


            Console.WriteLine($"Developers generated: {Seed.Developers.Count()}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error generating Developers: ", ex.Message);
            throw;
        }
    }

    private static void GenerateDesigners(ModelBuilder builder)
    {
        try
        {
            var professionsMap = Seed.Professions.ToDictionary(p => p.Name, p => p) ?? throw new Exception("Error generating Administrators, Professions null");
            var rolesMap = Seed.IdentityRoles.ToDictionary(r => r.Name, r => r) ?? throw new Exception("Error generating Staff, Roles are null");

            //✅ Seed Designers  
            var designers = new Faker<Designer>("en_ZA")
            .RuleFor(s => s.Id, f => Guid.NewGuid().ToString())
            .RuleFor(u => u.Gender, f => f.Person.Gender.ToString())
            .RuleFor(u => u.LastName, (f, u) => f.Name.LastName(u.Gender == nameof(Bogus.DataSets.Name.Gender.Male) ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female))
            .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName(u.Gender == nameof(Bogus.DataSets.Name.Gender.Male) ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female))
            .RuleFor(s => s.StaffNumber, (f, s) => $"{s.LastName.Substring(0, 1)}{s.FirstName.Substring(0, 1)}{f.Person.DateOfBirth.ToString("yy/MM/dd").Replace("/", "")}")
            .RuleFor(s => s.Email, (f, s) => f.Internet.Email(s.FirstName, s.LastName))
            .RuleFor(s => s.NormalizedEmail, (f, s) => s.Email.ToUpper())
            .RuleFor(s => s.UserName, (f, s) => s.Email)
            .RuleFor(s => s.NormalizedUserName, (f, s) => s.Email.ToUpper())
            .RuleFor(s => s.EmailConfirmed, f => true)
            .RuleFor(s => s.PhoneNumber, f => f.Phone.PhoneNumber("+27 ## ### ####"))
            .RuleFor(s => s.PasswordHash, (f, s) => new PasswordHasher<Designer>().HashPassword(s, "Designer@123"))
            .RuleFor(a => a.Bio, f => f.Lorem.Paragraphs(_random.Next(1, 3)))
            .RuleFor(s => s.ProfessionId, f =>
            {
                return f.PickRandom(professionsMap["Graphic Designer"].Id, professionsMap["Frontend Developer"].Id);
            })
            .RuleFor(a => a.Skills, (f, a) =>
            {
                var softSkills = Seed.SoftSkills.OrderBy(_ => Guid.NewGuid()).Take(_random.Next(1, Seed.SoftSkills.Count + 1)).ToList();
                foreach (var skill in softSkills)
                {
                    builder.Entity<Staff_Skill>().HasData(
                        new Staff_Skill { Id = Guid.NewGuid(), StaffId = a.Id, UserSkillId = skill.Id, YearsOfExperience = _random.Next(1, 6) }
                    );
                }

                var technicalSkills = Seed.TechnicalSkills.OrderBy(_ => Guid.NewGuid()).Take(_random.Next(1, Seed.TechnicalSkills.Count + 1)).ToList();
                foreach (var skill in technicalSkills)
                {
                    builder.Entity<Staff_Skill>().HasData(
                        new Staff_Skill { Id = Guid.NewGuid(), StaffId = a.Id, UserSkillId = skill.Id, YearsOfExperience = _random.Next(4, 10) }
                    );
                }
                return null;
            })
            .Generate(5);
            designers.ForEach(designer =>
            {
                builder.Entity<IdentityUserRole<string>>().HasData(
                    new IdentityUserRole<string> { UserId = designer.Id, RoleId = rolesMap[nameof(Staff)].Id },
                    new IdentityUserRole<string> { UserId = designer.Id, RoleId = rolesMap[nameof(Designer)].Id }
                );
                Seed.ApplicationUsers.Add(designer);
                Seed.Designers.Add(designer);
                builder.Entity<Designer>().HasData(designer);
            });

            Console.WriteLine($"Designers generated: {Seed.Designers.Count()}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error generating Designers: ", ex.Message);
            throw;
        }
    }

    private static void GenerateClients(ModelBuilder builder)
    {
        try
        {
            var userRolesMap = Seed.IdentityRoles.ToDictionary(r => r.Name, r => r) ?? throw new Exception("Error generating Clients, Roles are null");

            //✅ Seed Clients
            var clients = new Faker<Client>("en_ZA")
            .RuleFor(c => c.Id, f => Guid.NewGuid().ToString())
            .RuleFor(u => u.Gender, f => f.Person.Gender.ToString())
            .RuleFor(u => u.LastName, (f, u) => f.Name.LastName(u.Gender == nameof(Bogus.DataSets.Name.Gender.Male) ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female))
            .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName(u.Gender == nameof(Bogus.DataSets.Name.Gender.Male) ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female))
            .RuleFor(c => c.Email, (f, s) => f.Internet.Email(s.FirstName, s.LastName))
            .RuleFor(c => c.NormalizedEmail, (f, c) => c.Email.ToUpper())
            .RuleFor(c => c.UserName, (f, c) => c.Email)
            .RuleFor(c => c.NormalizedUserName, (f, c) => c.Email.ToUpper())
            .RuleFor(c => c.EmailConfirmed, f => true)
            .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber("+27 ## ### ####"))
            .RuleFor(c => c.PasswordHash, (f, c) => new PasswordHasher<Client>().HashPassword(c, "Client@123"))
            .RuleFor(s => s.Avatar, (f, s) =>
                $"https://randomuser.me/api/portraits/{(s.Gender == "Male" ? "men" : "women")}/{f.Random.Int(1, 100)}.jpg")
            .RuleFor(c => c.OrganizationId, (f, c) =>
            {
                switch (f.PickRandom(true, false))
                {
                    case true:
                        {
                            var organization = new Faker<Organization>("en_ZA")
                            .RuleFor(o => o.Id, f => Guid.NewGuid())
                            .RuleFor(o => o.Name, f => f.Company.CompanyName())
                            .Generate();

                            builder.Entity<Organization>().HasData(organization);
                            return organization.Id;
                        }
                    default:
                        break;
                }
                return null;
            })
            .RuleFor(a => a.Bio, f => f.Lorem.Paragraphs(_random.Next(1, 3)))
            .RuleFor(c => c.CreatedAt, f => f.Date.Past())
            .Generate(5);

            clients.ForEach(client =>
            {
                builder.Entity<IdentityUserRole<string>>().HasData(
                    new IdentityUserRole<string> { UserId = client.Id, RoleId = userRolesMap[nameof(Client)].Id }
                );
            });

            Seed.ApplicationUsers.AddRange(clients);
            Seed.Clients.AddRange(clients);
            builder.Entity<Client>().HasData(clients);

            Console.WriteLine($"Clients generated: {Seed.Clients.Count()}");

            GenerateBusinessServiceTestimonials(builder);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating Clients: {ex.Message}");
        }
    }

    private static void GenerateOtherUsers(ModelBuilder builder)
    {
        try
        {
            // ✅Other Users
            var users = new Faker<GuestUser>("en_ZA")
                .RuleFor(u => u.Id, f => Guid.NewGuid().ToString())
                .RuleFor(u => u.Gender, f => f.Person.Gender.ToString())
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName(u.Gender == nameof(Bogus.DataSets.Name.Gender.Male) ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female))
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName(u.Gender == nameof(Bogus.DataSets.Name.Gender.Male) ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female))
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                .RuleFor(u => u.NormalizedEmail, (f, u) => u.Email.ToUpper())
                .RuleFor(u => u.UserName, (f, u) => u.Email)
                .RuleFor(u => u.NormalizedUserName, (f, u) => u.Email.ToUpper())
                .RuleFor(u => u.EmailConfirmed, f => true)
                .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber("+27 ## ### ####"))
                .RuleFor(u => u.PasswordHash, (f, u) => new PasswordHasher<GuestUser>().HashPassword(u, "GuestUser@123"))
                .RuleFor(s => s.Avatar, (f, s) =>
                $"https://randomuser.me/api/portraits/{(s.Gender == "Male" ? "men" : "women")}/{f.Random.Int(1, 100)}.jpg")
                .RuleFor(a => a.Bio, f => f.Lorem.Paragraphs(_random.Next(1, 3)))
                .Generate(5)
                .ToList();

            Seed.ApplicationUsers.AddRange(users);
            Seed.GuestUsers.AddRange(users);
            builder.Entity<GuestUser>().HasData(users);

            Console.WriteLine($"Clients generated: {Seed.Clients.Count()}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error generating OtherUsers: ", ex.Message);
            throw;
        }
    }

    private static void GenerateBusinessServiceTestimonials(ModelBuilder builder)
    {
        try
        {
            var businessServices = Seed.BusinessServices.ToList() ?? throw new Exception("BusinessServices is null at GenerateBusinessServiceTestimonials()");
            foreach (var ourService in businessServices)
            {

                // Ensure Seed.Clients has data
                if (Seed.Clients.ToList() == null | !Seed.Clients.Any())
                    throw new InvalidOperationException("Seed.Clients is empty at GenerateBusinessServiceTestimonials()!");

                // Generate 10 fake reviews using Bogus
                var testimonials = new Faker<Testimonial>("en_ZA")
                    .RuleFor(r => r.Id, f => Guid.NewGuid())
                    .RuleFor(r => r.EvaluatorId, f => f.PickRandom(Seed.Clients).Id) // Dummy reviewer id
                    .RuleFor(r => r.Comment, f => f.Lorem.Paragraph())
                    .RuleFor(r => r.Rating, f => f.Random.Int(1, 5))
                    .RuleFor(r => r.CreatedAt, f => f.Date.Past())
                    .Generate(_random.Next(1, 5));
                if (testimonials is null) throw new Exception("Testimonials is null at GenerateBusinessServiceTestimonials()");
                // Seed the generated testimonials
                Seed.Testimonials.AddRange(testimonials);
                builder.Entity<Testimonial>().HasData(testimonials);


                // Create testimonial entries for the selected testimonials linking them to our service
                foreach (var testimonial in testimonials.ToList())
                {
                    var businessServiceTestimonial = new BusinessService_Testimonial
                    {
                        Id = Guid.NewGuid(),
                        BusinessServiceId = ourService.Id,
                        TestimonialId = testimonial.Id
                    };
                    // Seed the testimonials into the model
                    Seed.BusinessService_Testimonials.Add(businessServiceTestimonial);
                    builder.Entity<BusinessService_Testimonial>().HasData(businessServiceTestimonial);
                }
            }

            Console.WriteLine($"BusinessServiceTestimonials generated: {Seed.Testimonials.Count()}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating BusinessServiceTestimonials: {ex.Message}");
            throw;
        }
    }
}
