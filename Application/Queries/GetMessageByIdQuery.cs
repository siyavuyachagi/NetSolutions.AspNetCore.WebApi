using Application.DTOs.Response;
using MediatR;

namespace Application.Queries
{
    public class GetMessageByIdQuery : IRequest<MessageDto>
    {
        public Guid Id { get; set; }
    }
}
