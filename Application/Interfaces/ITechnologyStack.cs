using Application.DTOs.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITechnologyStack
    {
        Task<List<TechnologyStackDto>> GetTechnologyStacksAsync();
        Task<TechnologyStackDto> GetTechnologyStackAsync(Guid id);
        Task DeleteTechnologyStackAsync(Guid id);
        Task<TechnologyStackDto> CreateTechnologyStackAsync(TechnologyStack technologyStack);
    }
}
