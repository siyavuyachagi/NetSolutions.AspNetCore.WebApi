using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.TestData
{
    public class UserRolesData
    {
        public static void GenerateUserRoles(ModelBuilder builder)
        {
            try
            {
                //✅ Seed identity roles with Guid type
                var identityRoles = new List<IdentityRole>
            {
                new IdentityRole{ Id = Guid.NewGuid().ToString(), Name = nameof(Administrator) },
                new IdentityRole{ Id = Guid.NewGuid().ToString(), Name = nameof(Client) },
                new IdentityRole{ Id = Guid.NewGuid().ToString(), Name = nameof(Designer) },
                new IdentityRole{ Id = Guid.NewGuid().ToString(), Name = nameof(Developer) },
                new IdentityRole{ Id = Guid.NewGuid().ToString(), Name = nameof(Staff) },
            };
                Seed.IdentityRoles.AddRange(identityRoles);
                builder.Entity<IdentityRole>().HasData(identityRoles);

                Console.WriteLine($"UserRoles generated: {identityRoles.Count()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error generating UserRoles: ", ex.Message);
                throw;
            }
        }
    }
}

