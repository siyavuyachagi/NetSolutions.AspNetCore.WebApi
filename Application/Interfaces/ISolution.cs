
using Application.DTOs;
using Application.DTOs.Response;
using Application.Helpers;

namespace Application.Interfaces
{
    public interface ISolution
    {
        Task<List<SolutionDto>> GetSolutionsAsync();
        Task<SolutionDto> GetSolutionAsync(Guid id);
        Task<Result<FileDownloadResult>> DownloadSolutionZipAsync(Guid id);
    }
}
