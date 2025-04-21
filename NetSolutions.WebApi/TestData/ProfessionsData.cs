using Microsoft.EntityFrameworkCore;
using NetSolutions.WebApi.TestData;
using NetSolutions.WebApi.Models.Domain;

namespace NetSolutions.WebApi.TestData;

public class ProfessionsData
{
    public static void GenerateProfessions(ModelBuilder builder)
    {
        try
        {
            //✅ Seed Profession

            var professions = new List<Profession>
            {
                new Profession
                {
                    Id = Guid.NewGuid(),
                    Name = "Software Developer",
                    Description = "Develops, tests, and maintains software applications.",
                    EducationRequirements = "Bachelor’s in Computer Science or related field",
                    CertificationRequirements = "Microsoft Certified: Azure Developer Associate",
                    CoreCompetencies = "Problem-solving, Debugging, Team Collaboration",
                    AverageSalary = 80000m,
                    ExperienceYearsRequired = 2,
                    CareerPath = "Junior Developer -> Senior Developer -> Tech Lead",
                    Industry = "Information Technology",
                    IsActive = true
                },
                new Profession
                {
                    Id = Guid.NewGuid(),
                    Name = "Frontend Developer",
                    Description = "Builds the user interface and experience of web applications.",
                    EducationRequirements = "Degree or Certification in Web Development",
                    CertificationRequirements = "Google Mobile Web Specialist, Microsoft Frontend Certification",
                    CoreCompetencies = "UI/UX Design, Responsive Development, Performance Optimization",
                    AverageSalary = 75000m,
                    ExperienceYearsRequired = 2,
                    CareerPath = "Junior Frontend Dev -> Senior Frontend Dev -> UI/UX Architect",
                    Industry = "Information Technology",
                    IsActive = true
                },
                new Profession
                {
                    Id = Guid.NewGuid(),
                    Name = "Graphic Designer",
                    Description = "Builds graphics, create stunning visuals, posters and logos.",
                    EducationRequirements = "Degree or Certification in Web Development",
                    CertificationRequirements = "Google Mobile Web Specialist, Microsoft Frontend Certification",
                    CoreCompetencies = "Photoshop, Adobe, UI/UX Design",
                    AverageSalary = 75000m,
                    ExperienceYearsRequired = 2,
                    CareerPath = "Junior Frontend Dev -> Senior Frontend Dev -> UI/UX Architect",
                    Industry = "Information Technology",
                    IsActive = true
                },
                new Profession
                {
                    Id = Guid.NewGuid(),
                    Name = "HR Manager",
                    Description = "Oversees the HR department, manages policies and employee relations.",
                    EducationRequirements = "Bachelor’s in Human Resources or Business Administration",
                    CertificationRequirements = "PHR, SHRM-CP",
                    CoreCompetencies = "Employee Relations, Organizational Development, Compliance",
                    AverageSalary = 90000m,
                    ExperienceYearsRequired = 5,
                    CareerPath = "HR Specialist -> HR Manager -> HR Director",
                    Industry = "Human Resources",
                    IsActive = true
                },
                new Profession
                {
                    Id = Guid.NewGuid(),
                    Name = "Recruiter",
                    Description = "Sources, interviews, and hires candidates for job openings.",
                    EducationRequirements = "Bachelor’s in HR, Business, or Psychology",
                    CertificationRequirements = "Certified Professional Recruiter (CPR)",
                    CoreCompetencies = "Candidate Sourcing, Employer Branding, Onboarding",
                    AverageSalary = 70000m,
                    ExperienceYearsRequired = 3,
                    CareerPath = "Recruiter -> Senior Recruiter -> Talent Acquisition Manager",
                    Industry = "Human Resources",
                    IsActive = true
                }
            };

            Seed.Professions.AddRange(professions);
            builder.Entity<Profession>().HasData(professions);

            Console.WriteLine($"Staff Professions generated: {professions.Count()}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating Proffesioons: {ex.Message}, {ex.TargetSite.Name}");
            throw;
        }
    }
}
