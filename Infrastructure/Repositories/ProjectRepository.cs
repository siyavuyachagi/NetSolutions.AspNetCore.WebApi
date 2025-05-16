using Application.DTOs.Response;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class ProjectsRepository : IProject
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProjectsRepository> _logger;

        public ProjectsRepository(
            ApplicationDbContext context,
            IMapper mapper,
            ILogger<ProjectsRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result> DeleteProjectAsync(Guid Id)
        {
            try
            {
                await _context.Projects.Where(x => x.Id == Id).ExecuteDeleteAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
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
                    .Include(x => x.ProjectTeam.ProjectTeamMembers)
                    .ThenInclude(x => x.Member)
                    .Include(x => x.Project_TechnologyStacks)
                        .ThenInclude(x => x.TechnologyStack)
                    .Include(x => x.Solutions)
                    .Include(x => x.Project_Timeline)
                        .ThenInclude(x => x.Timeline)
                        .ThenInclude(x => x.Intervals)
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
                    .Include(x => x.ProjectMilestones)
                    .Include(x => x.Project_FileMetadata_Documents)
                        .ThenInclude(x => x.FileMetadata)
                    .Include(x => x.ProjectTeam)
                    .Include(x => x.ProjectTeam.ProjectTeamMembers)
                    //.ThenInclude(x => x.Member)
                    .Include(x => x.Project_TechnologyStacks)
                        .ThenInclude(x => x.TechnologyStack)
                    .Include(x => x.Project_Timeline)
                        .ThenInclude(x => x.Timeline)
                        .ThenInclude(x => x.Intervals)
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
