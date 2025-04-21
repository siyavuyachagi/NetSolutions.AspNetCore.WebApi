using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Models.DTOs;

namespace NetSolutions.WebApi.Repositories
{
    public interface ISolutionRepository
    {
        Task<List<SolutionDto>> GetSolutionsAsync();
        Task<SolutionDto> GetSolutionAsync(Guid Id);
    }

    public class SolutionRepository : ISolutionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SolutionRepository(
            ApplicationDbContext context, 
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SolutionDto> GetSolutionAsync(Guid Id)
        {
            var solution = await _context.Solutions
                .AsNoTrackingWithIdentityResolution()
                .Where(x => x.Id == Id)
                .Include(x => x.SolutionFeatures)
                .Include(x => x.Solution_TechnologyStacks)
                .ThenInclude(x => x.TechnologyStack)
                .Include(x => x.Solution_FileMetadata_Images)
                .ThenInclude(x => x.FileMetadata)
                .Include(x => x.Solution_FileMetadata_Documents)
                .ThenInclude(x => x.FileMetadata)
                .Include(x => x.Solution_Reviews)
                .ThenInclude(x => x.Review)
                .Include(x => x.Solution_Likes)
                .ThenInclude(x => x.Liker)
                .FirstOrDefaultAsync();

            var solutionDto = _mapper.Map<SolutionDto>(solution);
            return solutionDto;
        }

        public async Task<List<SolutionDto>> GetSolutionsAsync()
        {
            var solutions = await _context.Solutions
                .AsNoTrackingWithIdentityResolution()
                .Include(x => x.SolutionFeatures)
                .Include(x => x.Solution_TechnologyStacks)
                .ThenInclude(x => x.TechnologyStack)
                .Include(x => x.Solution_FileMetadata_Images)
                .ThenInclude(x => x.FileMetadata)
                .Include(x => x.Solution_FileMetadata_Documents)
                .ThenInclude(x => x.FileMetadata)
                .Include(x => x.Solution_Reviews)
                .ThenInclude(x => x.Review)
                .Include(x => x.Solution_Likes)
                .ThenInclude(x => x.Liker)
                .ToListAsync();

            var solutionsDto = _mapper.Map<List<SolutionDto>>(solutions);
            return solutionsDto;
        }
    }
}
