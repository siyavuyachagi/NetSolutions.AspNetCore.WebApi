using Bogus;
using Microsoft.EntityFrameworkCore;
using NetSolutions.WebApi.Models.Domain;

namespace NetSolutions.WebApi.TestData;

public class SubscriptionsData
{
    private static readonly Faker _faker = new Faker("en_ZA");
    private static Random _random = new Random();

    public static void GenerateUserSubscriptions(ModelBuilder builder)
    {
		try
		{
            var clients = Seed.Clients.ToList() ?? throw new Exception("Cannot find Seed.Clients");

            int max = (int)Math.Floor(clients.Count * 0.5); // Maximum: 50% of clients
            int count = _random.Next(1, max + 1); // Picks at least 1, up to 50%
            var randomClients = _faker.PickRandom(clients, count).ToList();

            var businessServicePackage = Seed.BusinessServicePackages.ToList() ?? throw new Exception("Cannot find Seed.BusinessServicePackages");

            foreach (var client in randomClients)
            {
                var selectedPackage = _faker.PickRandom(businessServicePackage);
                var subscription = new Subscription
                {
                    Id = Guid.NewGuid(),
                    ClientId = client.Id,
                    BusinessServicePackageId = selectedPackage.Id,
                    Status = Subscription.EStatus.Active,
                    //RecurringCycle = selectedPackage.BillingCycle,
                    CreatedAt = _faker.Date.Past()
                };

                Seed.Subscriptions.Add(subscription);
                builder.Entity<Subscription>().HasData(subscription);

                // Save transaction records
                var transaction = new PaymentTransaction
                {
                    Id = Guid.NewGuid(),
                    Amount = selectedPackage.Price,
                    Status = PaymentTransaction.EStatus.Complete,
                    TransactionReference = subscription.Id.ToString(),
                    PaymentMethod = _faker.PickRandom(new[] { "cc", "dc" }),
                    CreatedAt = subscription.CreatedAt
                };
                Seed.PaymentTransactions.Add(transaction);
                builder.Entity<PaymentTransaction>().HasData(transaction);
            }
            Console.WriteLine($"Subscriptions generated: {Seed.Projects.Count()}");
        }
        catch (Exception ex)
		{
			Console.WriteLine($"Error generating user subscriptions: ",ex.Message);
			throw;
		}
    }
}
