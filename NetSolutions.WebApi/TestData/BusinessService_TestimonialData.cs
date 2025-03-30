using Bogus;
using Microsoft.EntityFrameworkCore;
using NetSolutions.WebApi.Models.Domain;

namespace NetSolutions.WebApi.TestData;

public class BusinessService_TestimonialData
{
    private static Faker _faker = new("en_ZA");
    public static void GenerateBusinessServiceTestimonials(ModelBuilder builder)
    {
        try
        {
            var businessServices = Seed.BusinessServices.ToList() ?? throw new Exception("BusinessServices is null at GenerateBusinessServiceTestimonials()");
            foreach (var ourService in businessServices)
            {

                // Ensure Seed.Clients has data
                if (Seed.Clients.ToList() == null | !Seed.Clients.Any())
                    throw new InvalidOperationException("Seed.Clients is empty!");

                // Generate 10 fake reviews using Bogus
                var testimonials = new Faker<Testimonial>("en_ZA")
                    .RuleFor(r => r.Id, f => Guid.NewGuid())
                    .RuleFor(r => r.EvaluatorId, f => f.PickRandom(Seed.Clients).Id) // Dummy reviewer id
                    .RuleFor(r => r.Comment, f => f.Lorem.Paragraph())
                    .RuleFor(r => r.Rating, f => f.Random.Int(1, 5))
                    .RuleFor(r => r.CreatedAt, f => f.Date.Past())
                    .Generate(5);
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

            Console.WriteLine($"Testimonials: {Seed.Testimonials.Count()}");
            Console.WriteLine("GenerateBusinessServiceTestimonials Complete");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating BusinessServiceTestimonials: {ex.Message}");
            throw;
        }
    }
}
