using NetSolutions.WebApi.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace NetSolutions.WebApi.Models.DTOs;

public class UserSkillDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<StaffDto> Staff { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
