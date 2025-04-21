using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using NetSolutions.Helpers;

namespace NetSolutions.Services;

public interface IEmailSender
{
    Task<Result> SendEmailAsync(string to, string subject, string htmlBody);
    Task<Result> SendEmailAsync(string from, string to, string subject, string htmlBody);
    Task<Result> SendEmailAsync(string from, string to, string subject, string htmlBody, IFormFile attachment);
    Task<Result> SendEmailAsync(string from, string to, string subject, string htmlBody, List<IFormFile> attachments);
}

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _emailSettings;

    public EmailSender(EmailSettings emailSettings)
    {
        _emailSettings = emailSettings;
    }

    public async Task<Result> SendEmailAsync(string from, string to, string subject, string htmlBody)
    {
        var message = CreateEmailMessage(from, from, to, subject, htmlBody);
        return await SendAsync(message);
    }

    public async Task<Result> SendEmailAsync(string from, string to, string subject, string htmlBody, IFormFile attachment)
    {
        var message = CreateEmailMessage(from, from, to, subject, htmlBody, new List<IFormFile> { attachment });
        return await SendAsync(message);
    }

    public async Task<Result> SendEmailAsync(string from, string to, string subject, string htmlBody, List<IFormFile> attachments)
    {
        var message = CreateEmailMessage(from, from, to, subject, htmlBody, attachments);
        return await SendAsync(message);
    }

    public async Task<Result> SendEmailAsync(string to, string subject, string htmlBody)
    {
        var message = CreateEmailMessage(_emailSettings.DisplayName, _emailSettings.Email, to, subject, htmlBody);
        return await SendAsync(message);
    }

    private MimeMessage CreateEmailMessage(string Name, string from, string to, string subject, string htmlBody, List<IFormFile>? attachments = null)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(Name, from));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;

        var builder = new BodyBuilder { HtmlBody = htmlBody };

        if (attachments != null && attachments.Any())
        {
            foreach (var file in attachments)
            {
                using var ms = new MemoryStream();
                file.CopyTo(ms);
                ms.Position = 0;
                builder.Attachments.Add(file.FileName, ms.ToArray(), ContentType.Parse(file.ContentType));
            }
        }

        email.Body = builder.ToMessageBody();
        return email;
    }

    private async Task<Result> SendAsync(MimeMessage mail)
    {
        try
        {
            using var client = new SmtpClient();
            await client.ConnectAsync(_emailSettings.Server, _emailSettings.Port, SecureSocketOptions.Auto);
            await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
            await client.SendAsync(mail);
            await client.DisconnectAsync(true);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failed(ex.Message);
        }
    }
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


