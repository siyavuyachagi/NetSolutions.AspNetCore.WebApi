using NetSolutions.WebApi.Models.DTOs;

namespace NetSolutions.WebApi.Repositories
{
    public interface ISubscriptionRepository
    {
        Task<List<SubscriptionDto>> GetSubscriptionsAsync();
        Task<SubscriptionDto> GetSubscriptionAsync(Guid Id);
    }

    public class SubscriptionRepository : ISubscriptionRepository
    {
        public Task<SubscriptionDto> GetSubscriptionAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<List<SubscriptionDto>> GetSubscriptionsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
