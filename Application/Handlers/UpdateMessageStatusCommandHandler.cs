using Application.Commands;
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Handlers
{
    public class UpdateMessageStatusCommandHandler : IRequestHandler<UpdateMessageStatusCommand, MessageDto>
    {
        private readonly IMessages _messagesService;
        private readonly IMapper _mapper;

        public UpdateMessageStatusCommandHandler(IMessages messagesService, IMapper mapper)
        {
            _messagesService = messagesService;
            _mapper = mapper;
        }

        public async Task<MessageDto> Handle(UpdateMessageStatusCommand request, CancellationToken cancellationToken)
        {
            var message = await _messagesService.GetByIdAsync(request.MessageId);

            if (message == null)
                throw new ArgumentNullException($"Message with ID {request.MessageId} not found.");

            message.Status = request.NewStatus;

            var updatedMessage = await _messagesService.UpdateAsync(message);
            return _mapper.Map<MessageDto>(updatedMessage);
        }
    }
}
