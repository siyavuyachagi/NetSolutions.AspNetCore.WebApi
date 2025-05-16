using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class ClientRepositotry : IClient
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ClientRepositotry(
            ApplicationDbContext context, 
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ClientDto> GetClientAsync(string Id)
        {
            var client = await _context.Clients
                .AsNoTrackingWithIdentityResolution()
                .Where(x => x.Id == Id)
                .Include(x => x.IdentityUserRoles)
                .Include(x => x.IdentityRoles)
                .Include(x => x.PhysicalAddress)
                .Include(x => x.UserActivities)
                .Include(x => x.Subscription)
                .Include(x => x.Projects)
                .Include(x => x.User_Solutions)
                .ThenInclude(x => x.Solution)
                .FirstOrDefaultAsync();

            var result = _mapper.Map<ClientDto>(client);
            return result;
        }

        public async Task<List<ClientDto>> GetClientsAsync()
        {
            var clients = await _context.Clients
                .AsNoTrackingWithIdentityResolution()
                .Include(x => x.IdentityUserRoles)
                .Include(x => x.IdentityRoles)
                .Include(x => x.PhysicalAddress)
                .Include(x => x.UserActivities)
                .ToListAsync();

            var results = _mapper.Map<List<ClientDto>>(clients);
            return results;
        }
    }
}
