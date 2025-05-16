using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectTeamMembersController : ControllerBase
    {
        private readonly ILogger<ProjectTeamMembersController> _logger;
        private readonly IProjectTeamMember _projectTeamMemberRepository;

        public ProjectTeamMembersController(
            ILogger<ProjectTeamMembersController> logger, 
            IProjectTeamMember projectTeamMemberRepository)
        {
            _logger = logger;
            _projectTeamMemberRepository = projectTeamMemberRepository;
        }




        public class AddTeamMemberRequest
        {
            [Required]
            public Guid ProjectTeamId { get; set; }
            [Required]
            public string MemberId { get; set; }
            [MinLength(1)]
            public List<Guid> TeamMemberRoleIdList { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddTeamMemberRequest request)
        {
            try
            {
                if (ModelState.IsValid) return ValidationProblem(ModelState);

                var projectTeam = await _projectTeamMemberRepository.GetProjectTeamMemberAsync(request.ProjectTeamId);
                if (projectTeam is null) BadRequest();

                var projectTeamMember = new ProjectTeamMember
                {
                    MemberId = request.MemberId,
                    ProjectTeamMember_TeamMemberRoles = new(),
                };

                foreach (var roleId in request.TeamMemberRoleIdList)
                {
                    projectTeamMember.ProjectTeamMember_TeamMemberRoles.Add(new ProjectTeamMember_TeamMemberRole
                    {
                        TeamMemberRoleId = roleId,
                    });
                }

                var result = await _projectTeamMemberRepository.CreateProjectTeamMemberAsync(projectTeamMember);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }


        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid Id, [FromBody] string memberId)
        {
            try
            {
                if (ModelState.IsValid) return ValidationProblem();

                await _projectTeamMemberRepository.DeleteProjectTeamAsync(Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
