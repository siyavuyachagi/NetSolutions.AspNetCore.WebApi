using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Models.DTOs;

namespace NetSolutions.WebApi.Repositories
{
    public interface IProjectRepository
    {
        Task<List<ProjectDto>> GetProjectsAsync();
        Task<ProjectDto> GetProjectAsync(Guid Id);
    }


    public class ProjectsRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProjectsRepository(
            ApplicationDbContext context, 
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProjectDto> GetProjectAsync(Guid Id)
        {
            try
            {
                var project = await _context.Projects
                    .AsNoTrackingWithIdentityResolution()
                    .Where(x => x.Id == Id)
                    .Include(x => x.BusinessService)
                    .Include(x => x.Client)
                    .Include(x => x.ProjectTasks)
                    .Include(x => x.ProjectMilestones)
                    .Include(x => x.Project_FileMetadata_Documents)
                    .ThenInclude(x => x.FileMetadata)
                    .Include(x => x.ProjectTeam)
                    .Include(x => x.Project_TechnologyStacks)
                    .ThenInclude(x => x.TechnologyStack)
                    .FirstOrDefaultAsync();

                var projectDto = _mapper.Map<ProjectDto>(project);
                return projectDto;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<ProjectDto>> GetProjectsAsync()
        {
            try
            {
                var projects = await _context.Projects
                    .AsNoTrackingWithIdentityResolution()
                    .Include(x => x.BusinessService)
                    .Include(x => x.Client)
                    .Include(x => x.ProjectTasks)
                    //.Include(x => x.ProjectMilestones)
                    .Include(x => x.Project_FileMetadata_Documents)
                    .ThenInclude(x => x.FileMetadata)
                    .Include(x => x.ProjectTeam)
                    .Include(x => x.Project_TechnologyStacks)
                    .ThenInclude(x => x.TechnologyStack)
                    .ToListAsync();

                var projectsDto = _mapper.Map<List<ProjectDto>>(projects);
                return projectsDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
