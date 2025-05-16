namespace Application.DTOs.Response
{
    public class Solution_LikeDto
    {
        public Guid Id { get; set; }
        public virtual SolutionDto Solution { get; set; }
        public virtual ApplicationUserDto Liker { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

