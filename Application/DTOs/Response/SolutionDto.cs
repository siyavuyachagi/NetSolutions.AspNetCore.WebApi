namespace Application.DTOs.Response
{
    public class SolutionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ProjectDto Project { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string SourceUrl { get; set; }
        public string PreviewUrl { get; set; }
        public string Version { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Discriminator { get; set; }
        public List<SolutionFeatureDto> Features { get; set; }
        public List<TechnologyStackDto> TechnologyStacks { get; set; }
        public List<FileMetadataDto> Images { get; set; }
        public List<FileMetadataDto> Documents { get; set; }
        public List<ReviewDto> Reviews { get; set; }
        public List<Solution_LikeDto> Likes { get; set; }
    }
}


