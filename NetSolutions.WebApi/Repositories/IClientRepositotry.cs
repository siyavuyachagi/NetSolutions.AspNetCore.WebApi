using NetSolutions.Helpers;
using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Models.DTOs;
using NetSolutions.WebApi.Services;

namespace NetSolutions.WebApi.Repositories;

public interface IClientRepositotry
{
    Task<Result<List<ClientDto>>> GetClientsAsync();
    Task<Result<ClientDto?>> GetClientAsync(string Id);
    Task<Result<ClientDto?>> GetClientByUserNameAsync(string UserName);
    Task<Result> DeleteClientAsync(string Id);
}


public class ClientRepositotry : IClientRepositotry
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ClientRepositotry> _logger;
    private readonly IRedisCache _redisCache;
    private const string CLIENTS_CACHE_KEY = "clients_list_cache";

    public Task<Result> DeleteClientAsync(string Id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<ClientDto?>> GetClientAsync(string Id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<ClientDto?>> GetClientByUserNameAsync(string UserName)
    {
        throw new NotImplementedException();
    }

    public Task<Result<List<ClientDto>>> GetClientsAsync()
    {
        throw new NotImplementedException();
    }
}