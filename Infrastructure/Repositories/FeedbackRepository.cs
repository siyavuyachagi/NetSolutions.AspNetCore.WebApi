using Application.DTOs.Response;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class FeedbackRepository : IFeedback
    {
        private readonly ILogger<FeedbackRepository> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public FeedbackRepository(
            ILogger<FeedbackRepository> logger, 
            ApplicationDbContext context, 
            IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> DeleteFeedbackAsync(Guid Id)
        {
            try
            {
                await _context.Feedbacks.Where(x => x.Id == Id).ExecuteDeleteAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public Task<FeedbackDto> GetFeedbackAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<List<FeedbackDto>> GetFeedbacksAsync()
        {
            throw new NotImplementedException();
        }
    }
}
