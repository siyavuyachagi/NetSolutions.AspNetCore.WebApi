
namespace Application.DTOs.Response
{
    public class TimelineDto
    {
        public Guid Id { get; set; }
        public virtual List<TimelineIntervalDto> Intervals { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
