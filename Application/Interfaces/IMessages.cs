using Domain.Entities;

namespace Application.Interfaces
{
    public interface IMessages
    {
        Task<Message> GetByIdAsync(Guid id);
        Task<IEnumerable<Message>> GetConversationAsync(string user1Id, string user2Id);
        Task<IEnumerable<Message>> GetUserMessagesAsync(string userId);
        Task<Message> AddAsync(Message message);
        Task<Message> UpdateAsync(Message message);
    }
}
