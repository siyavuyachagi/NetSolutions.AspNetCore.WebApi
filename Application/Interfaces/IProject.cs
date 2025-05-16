
using Application.DTOs.Response;
using Application.Helpers;

namespace Application.Interfaces
{
    public interface IProject
    {
        Task<List<ProjectDto>> GetProjectsAsync();
        Task<ProjectDto> GetProjectAsync(Guid Id);
        Task<Result> DeleteProjectAsync(Guid Id);
    }
}
