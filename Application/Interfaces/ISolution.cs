
using Application.DTOs.Response;

namespace Application.Interfaces
{
    public interface ISolution
    {
        Task<List<SolutionDto>> GetSolutionsAsync();
        Task<SolutionDto> GetSolutionAsync(Guid Id);
    }
}
