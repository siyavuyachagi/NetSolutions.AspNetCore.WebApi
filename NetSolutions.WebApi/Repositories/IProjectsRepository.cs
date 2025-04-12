using Microsoft.EntityFrameworkCore;
using NetSolutions.Helpers;
using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Models.Domain;
using NetSolutions.WebApi.Models.DTOs;
using NetSolutions.WebApi.Services;

namespace NetSolutions.WebApi.Repositories;

public interface IProjectsRepository
{
    Task<Result<List<ProjectDto>>> GetProjectsAsync();
    Task<Result<ProjectDto?>> GetProjectAsync(Guid Id);
    Task<Result> DeleteProjectAsync(Guid Id);
}


public class ProjectsRepository : IProjectsRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProjectsRepository> _logger;
    private readonly IRedisCache _redisCache;
    private const string PROJECTS_CACHE_KEY = "projects_list_cache";
    public ProjectsRepository(
        ApplicationDbContext context,
        ILogger<ProjectsRepository> logger,
        IRedisCache redisCache)
    {
        _context = context;
        _logger = logger;
        _redisCache = redisCache;
    }

    public async Task<Result> DeleteProjectAsync(Guid Id)
    {
        try
        {
            await _context.Projects
            .Where(u => u.Id == Id)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(u => u.IsDeleted, true));

            //Refresh cache
            await GetProjectsAsync();

            //Return success
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Failed(ex.Message);
        }
    }

    public async Task<Result<ProjectDto?>> GetProjectAsync(Guid Id)
    {
        try
        {
            var projectsCache = await _redisCache.GetAsync<List<ProjectDto>>(PROJECTS_CACHE_KEY) ?? [];
            if (projectsCache.Count != 0)
            {
                var project = projectsCache.Where(p => p.Id == Id).FirstOrDefault();
                return Result.Success(project);
            }
            else
            {
                var projects = await GetProjectsAsync();
                var project = projects.Response.Where(p => p.Id == Id).FirstOrDefault();
                return Result.Success(project);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return (Result<ProjectDto?>)Result.Failed(ex.Message);
        }
    }

    public async Task<Result<List<ProjectDto>>> GetProjectsAsync()
    {
        try
        {
            var projects = await _context.Projects
                .AsNoTrackingWithIdentityResolution()
                .Where(x => !x.IsDeleted)
                .Include(p => p.Client)
                .Include(p => p.Documents)  // Including the junction table
                    .ThenInclude(pd => pd.FileMetadata)  // Include related FileMetadata entities
                .Include(p => p.Team)
                    .ThenInclude(t => t.TeamMembers)
                        .ThenInclude(tm => tm.Member)
                .Include(p => p.Solutions.Where(s => !s.IsDeleted))
                .Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Budget = p.Budget,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    BusinessService = p.BusinessService,
                    Client = p.Client,
                    ProjectTasks = p.ProjectTasks,
                    ProjectMilestones = p.ProjectMilestones,
                    Solutions = p.Solutions,
                    Status = EnumHelper.GetDisplayName(p.Status),
                    Documents = p.Documents.Select(d => d.FileMetadata).ToList(),  // Select FileMetadata from the junction table
                    TechnologyStacks = p.TechnologyStacks.Select(ts => ts.TechnologyStack).ToList(),
                    ProjectTeam = new ProjectTeamDto
                    {
                        Id = p.Team.Id,
                        Name = p.Team.Name,
                        ProjectTeamMembers = p.Team.TeamMembers.Select(tm => new ProjectTeamMemberDto
                        {
                            Id = tm.Id,
                            UserId = tm.Member.Id,
                            FirstName = tm.Member.FirstName,
                            LastName = tm.Member.LastName,
                            UserName = tm.Member.UserName,
                            Email = tm.Member.Email,
                            CreatedAt = tm.CreatedAt,
                            UpdatedAt = tm.CreatedAt,
                            TeamMemberRoles = tm.Roles.Select(r => r.TeamMemberRole).ToList(),
                        }).ToList()
                    },
                    Discriminator = EF.Property<string>(p, "Discriminator")
                        .ToFormattedString(Casing.Pascal)
                        .Replace(nameof(Project), "")
                })
                .ToListAsync();

            // Save cache
            if (projects.Count != 0)
                await _redisCache.SetAsync(PROJECTS_CACHE_KEY, projects);

            // return results
            return Result.Success(projects);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await _redisCache.RemoveAsync(PROJECTS_CACHE_KEY);
            return (Result<List<ProjectDto>>)Result.Failed(ex.Message);
        }
    }
}
