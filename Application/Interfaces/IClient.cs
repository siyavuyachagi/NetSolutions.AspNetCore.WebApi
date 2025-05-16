
using Application.DTOs.Response;

namespace Application.Interfaces
{
    public interface IClient
    {
        Task<List<ClientDto>> GetClientsAsync();
        Task<ClientDto> GetClientAsync(string Id);
    }
}
