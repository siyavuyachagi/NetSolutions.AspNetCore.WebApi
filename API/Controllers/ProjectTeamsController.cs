using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectTeamsController : ControllerBase
    {
        private readonly ILogger<ProjectTeamsController> _logger;
        private readonly IProjectTeam _projectTeamRepository;
        private readonly ApplicationDbContext _context;

        public ProjectTeamsController(
            ILogger<ProjectTeamsController> logger,
            IProjectTeam projectTeamRepository,
            ApplicationDbContext context)
        {
            _logger = logger;
            _projectTeamRepository = projectTeamRepository;
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var projectTeams = await _projectTeamRepository.GetProjectTeamsAsync();
                return Ok(projectTeams);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }


        [HttpGet("{Id}")]
        public async Task<IActionResult> Details([FromRoute] Guid Id)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);
                var projectTeam = await _projectTeamRepository.GetProjectTeamAsync(Id);
                return Ok(projectTeam);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }


        //public class ProjectTeamModel
        //{

        //}
        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] ProjectTeamModel model)
        //{
        //    try
        //    {
        //        var projectTeam = new ProjectTeam
        //        {

        //        };
        //        var result = await _projectTeamRepository.CreateProjectTeamAsync(projectTeam);
        //        return Created(result.Id.ToString(), result);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //        throw;
        //    }
        //}

        //[HttpDelete("{Id}")]
        //public async Task<IActionResult> Delete([FromRoute] Guid Id)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        //        await _projectTeamRepository.DeleteProjectTeamAsync(Id);
        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //        throw;
        //    }
        //}

        //public class AddTeamMemberRequest
        //{
        //    [Required]
        //    public Guid ProjectTeamId { get; set; }
        //    [Required]
        //    public string MemberId { get; set; }
        //    [MinLength(1)]
        //    public List<Guid> TeamMemberRoleIdList { get; set; }
        //}
        //[HttpPost("member")]
        //public async Task<IActionResult> AddTeamMember([FromBody] AddTeamMemberRequest request)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid) return ValidationProblem(ModelState);

        //        var projectTeam = await _context.ProjectTeams
        //            .Where(x => x.Id == request.ProjectTeamId)
        //            .Include(x => x.ProjectTeamMembers)
        //            .FirstOrDefaultAsync();

        //        if (projectTeam is null) BadRequest();

        //        var projectTeamMember = new ProjectTeamMember
        //        {
        //            MemberId = request.MemberId,
        //            ProjectTeamMember_TeamMemberRoles = new(),
        //        };

        //        foreach (var roleId in request.TeamMemberRoleIdList)
        //        {
        //            projectTeamMember.ProjectTeamMember_TeamMemberRoles.Add(new ProjectTeamMember_TeamMemberRole
        //            {
        //                TeamMemberRoleId = roleId,
        //            });
        //        }

        //        projectTeam.ProjectTeamMembers.Add(projectTeamMember);

        //        await _context.SaveChangesAsync();
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //        throw;
        //    }
        //}


        //[HttpDelete("{Id}/member")]
        //public async Task<IActionResult> Delete([FromRoute] Guid Id, [FromBody] string memberId)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid) return ValidationProblem();

        //        int affectedRows = await _context.ProjectTeamMembers
        //            .Where(x => x.Id == Id)
        //            .ExecuteDeleteAsync();
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //        throw;
        //    }
        //}
    }
}
