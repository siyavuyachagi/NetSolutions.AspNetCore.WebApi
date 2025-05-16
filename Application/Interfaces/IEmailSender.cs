using Application.Helpers;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IEmailSender
    {
        Task<Result> SendEmailAsync(string to, string subject, string htmlBody);
        Task<Result> SendEmailAsync(string from, string to, string subject, string htmlBody);
        Task<Result> SendEmailAsync(string from, string to, string subject, string htmlBody, IFormFile attachment);
        Task<Result> SendEmailAsync(string from, string to, string subject, string htmlBody, List<IFormFile> attachments);
    }

    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
    }

    public class EmailSettings
    {
        public string Server { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}


