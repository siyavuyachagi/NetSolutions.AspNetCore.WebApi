using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetSolutions.WebApi.Models.DTOs;

public class SolutionLikeDto
{
    public Guid Id { get; set; }
    public virtual SolutionDto Solution { get; set; }
    public virtual ApplicationUserDto Liker { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
