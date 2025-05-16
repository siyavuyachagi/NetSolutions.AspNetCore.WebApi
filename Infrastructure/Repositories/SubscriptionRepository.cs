using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SubscriptionRepository : ISubscription
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SubscriptionRepository(
            ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateSubscriptionAsync(Subscription subscription)
        {
            try
            {
                _context.Add(subscription);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<SubscriptionDto> GetSubscriptionAsync(Guid Id)
        {
            var subscription = await _context.Subscriptions
                .AsNoTrackingWithIdentityResolution()
                .Where(x => x.Id == Id)
                .Include(x => x.Client)
                .Include(x => x.BusinessServicePackage)
                .FirstOrDefaultAsync();

            var dto = _mapper.Map<SubscriptionDto>(subscription);
            return dto;
        }

        public async Task<List<SubscriptionDto>> GetSubscriptionsAsync()
        {
            var subscriptions = await _context.Subscriptions
                .AsNoTrackingWithIdentityResolution()
                .Include(x => x.Client)
                .Include(x => x.BusinessServicePackage)
                .ToListAsync();

            var dto = _mapper.Map<List<SubscriptionDto>>(subscriptions);
            return dto;
        }

        public async Task UpdateStatusAsync(Guid id, Subscription.EStatus status)
        {
            try
            {
                var subscription = await _context.Subscriptions.FindAsync(id);
                subscription.Status = status;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
