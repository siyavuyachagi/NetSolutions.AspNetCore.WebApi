using Application.DTOs.Response;
using Domain.Entities;
using MediatR;

namespace Application.Commands
{
    public class UpdateMessageStatusCommand : IRequest<MessageDto>
    {
        public Guid MessageId { get; set; }
        public Message.EStatus NewStatus { get; set; }
    }
}
