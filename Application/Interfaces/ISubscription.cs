using Application.DTOs.Response;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ISubscription
    {
        Task<List<SubscriptionDto>> GetSubscriptionsAsync();
        Task<SubscriptionDto> GetSubscriptionAsync(Guid id);
        Task CreateSubscriptionAsync(Subscription subscription);
        Task UpdateStatusAsync(Guid id, Subscription.EStatus status);
    }
}
