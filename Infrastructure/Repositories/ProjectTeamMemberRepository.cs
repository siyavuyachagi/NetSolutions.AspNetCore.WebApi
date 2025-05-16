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
    public class ProjectTeamMemberRepository : IProjectTeamMember
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProjectTeamMemberRepository> _logger;

        public ProjectTeamMemberRepository(
            ApplicationDbContext context,
            IMapper mapper,
            ILogger<ProjectTeamMemberRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ProjectTeamMemberDto> CreateProjectTeamMemberAsync(ProjectTeamMember projectTeamMember)
        {
            _context.ProjectTeamMembers.Add(projectTeamMember);
            await _context.SaveChangesAsync();
            return await GetProjectTeamMemberAsync(projectTeamMember.Id);
        }

        public async Task DeleteProjectTeamAsync(Guid id)
        {
            await _context.ProjectTeamMembers
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task<ProjectTeamMemberDto> GetProjectTeamMemberAsync(Guid Id)
        {
            var projectTeamMember = await _context.ProjectTeamMembers
                .AsNoTrackingWithIdentityResolution()
                .Where(x => x.Id == Id)
                .FirstOrDefaultAsync();

            var result = _mapper.Map<ProjectTeamMemberDto>(projectTeamMember);
            return result;
        }

        public async Task<List<ProjectTeamMemberDto>> GetProjectTeamMembersAsync()
        {
            var projectTeamMember = await _context.ProjectTeamMembers
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefaultAsync();

            var result = _mapper.Map<List<ProjectTeamMemberDto>>(projectTeamMember);
            return result;
        }
    }
}
