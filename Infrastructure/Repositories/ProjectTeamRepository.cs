using Application.DTOs.Response;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class ProjectTeamRepository : IProjectTeam
    {
        private readonly ILogger<ProjectTeamRepository> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProjectTeamRepository(
            ILogger<ProjectTeamRepository> logger,
            ApplicationDbContext context,
            IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProjectTeamDto> CreateProjectTeamAsync(ProjectTeam projectTeam)
        {
            try
            {
                _context.ProjectTeams.Add(projectTeam);
                await _context.SaveChangesAsync();

                return await GetProjectTeamAsync(projectTeam.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<Result> DeleteProjectTeamAsync(Guid Id)
        {
            try
            {
                await _context.ProjectTeams.Where(x => x.Id == Id)
                    .ExecuteDeleteAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<ProjectTeamDto> GetProjectTeamAsync(Guid Id)
        {
            try
            {
                var projectTeam = await _context.ProjectTeams
                    .AsNoTrackingWithIdentityResolution()
                    .Where(x => x.Id == Id)
                    .Include(x => x.Project)
                    .Include(x => x.ProjectTeamMembers)
                    .ThenInclude(x => x.Member)
                    .Include(x => x.ProjectTeamMembers)
                    .ThenInclude(x => x.ProjectTeamMember_TeamMemberRoles)
                    .ThenInclude(x => x.TeamMemberRole)
                    .ToListAsync();

                var response = _mapper.Map<ProjectTeamDto>(projectTeam);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<List<ProjectTeamDto>> GetProjectTeamsAsync()
        {
            try
            {
                var projectTeams = await _context.ProjectTeams
                    .AsNoTrackingWithIdentityResolution()
                    .Include(x => x.Project)
                    .Include(x => x.ProjectTeamMembers)
                    .ToListAsync();

                var response = _mapper.Map<List<ProjectTeamDto>>(projectTeams);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
