using Application.Commands;
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Handlers
{
    public class SendMessageHandler : IRequestHandler<SendMessageCommand, MessageDto>
    {
        private readonly IMessages _messageRepository;
        private readonly IMapper _mapper;

        public SendMessageHandler(IMessages messageRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        public async Task<MessageDto> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var message = new Message(request.Text, request.SenderId, request.ReceiverId);
            message.SentAt = DateTime.UtcNow;
            message.UpdatedAt = DateTime.UtcNow;

            await _messageRepository.AddAsync(message);
            return _mapper.Map<MessageDto>(message);
        }
    }
}
