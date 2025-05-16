namespace Application.DTOs.Response
{
    public class ClientDto : ApplicationUserDto
    {
        public SubscriptionDto? Subscription { get; set; }
        public List<ProjectDto> Projects { get; set; }
        public List<SolutionDto> Solutions { get; set; }
    }

}

