using Domain.Entities;

namespace Application.DTOs.Response
{
    public class FeedbackDto
    {
        public Guid Id { get; set; }
        public string EvaluatorId { get; set; }
        public ApplicationUserDto Evaluator { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
