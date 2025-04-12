using NetSolutions.WebApi.Models.Domain;

namespace NetSolutions.WebApi.Models.DTOs;

public class SolutionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Project Project { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string SourceUrl { get; set; }
    public string PreviewUrl { get; set; }
    public string Version { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Discriminator { get; set; }
    public List<SolutionFeature> Features { get; set; }
    public List<TechnologyStack> TechnologyStacks { get; set; }
    public List<FileMetadata> Images { get; set; }
    public List<FileMetadata> Documents { get; set; }
    public List<Review> Reviews { get; set; }
    public List<Solution_Like> Likes { get; set; }
}

