namespace Application.DTOs.Response
{
    public class SolutionFeatureDto
    {
        public Guid Id { get; set; }
        public virtual SolutionDto Solution { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
