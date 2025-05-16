using Application.DTOs.Response;
using Application.Helpers;

namespace Application.Interfaces
{
    public interface IFeedback
    {
        Task<List<FeedbackDto>> GetFeedbacksAsync();
        Task<FeedbackDto> GetFeedbackAsync(Guid Id);
        Task<Result> DeleteFeedbackAsync(Guid Id);
    }
}
