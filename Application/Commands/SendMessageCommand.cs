using Application.DTOs.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class SendMessageCommand : IRequest<MessageDto>
    {
        public string Text { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public List<FileMetadataDto>? Attachments { get; set; }
    }
}
