using Application.DTOs.Response;
using Application.Interfaces;
using Application.Queries;
using AutoMapper;
using MediatR;

namespace Application.Handlers
{
    public class GetMessageByIdHandler : IRequestHandler<GetMessageByIdQuery, MessageDto>
    {
        private readonly IMessages _messageRepository;
        private readonly IMapper _mapper;

        public GetMessageByIdHandler(IMessages messageRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        public async Task<MessageDto> Handle(GetMessageByIdQuery request, CancellationToken cancellationToken)
        {
            var message = await _messageRepository.GetByIdAsync(request.Id);

            if (message == null)
                return null;

            return _mapper.Map<MessageDto>(message);
        }
    }
}
