using Application.DTOs.Response;
using MediatR;

namespace Application.Queries
{
    public class GetConversationQuery : IRequest<IEnumerable<MessageDto>>
    {
        public string User1Id { get; set; }
        public string User2Id { get; set; }
    }
}
