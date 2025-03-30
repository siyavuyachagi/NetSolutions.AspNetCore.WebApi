using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using NetSolutions.Helpers;

namespace NetSolutions.Services;
public interface IEmailSender
{
    Task<Result> SendEmailAsync(string from, string to, string subject, string htmlBody);
    Task<Result> SendEmailAsync(string from, string to, string subject, string htmlBody, IFormFile attachment);
    Task<Result> SendEmailAsync(string from, string to, string subject, string htmlBody, List<IFormFile> attachments);
}




public class EmailSender : IEmailSender
{
    private readonly SmtpSettings _smtpSettings;

    public EmailSender(
        IOptions<SmtpSettings> smtpSettings // ✅ Inject as IOptions<T>
    )
    {
        _smtpSettings = smtpSettings.Value; // ✅ Access the actual object
    }

    public async Task<Result> SendEmailAsync(string from, string to, string subject, string htmlBody)
    {
        var message = CreateEmailMessage(from, to, subject, htmlBody);
        await SendEmailAsync(message);
        return Result.Success();
    }

    public async Task<Result> SendEmailAsync(string from, string to, string subject, string htmlBody, IFormFile attachment)
    {
        var message = CreateEmailMessage(from, to, subject, htmlBody);
        AttachFileToEmail(message, attachment);
        await SendEmailAsync(message);
        return Result.Success();
    }

    public async Task<Result> SendEmailAsync(string from, string to, string subject, string htmlBody, List<IFormFile> attachments)
    {
        var message = CreateEmailMessage(from, to, subject, htmlBody);
        foreach (var file in attachments)
        {
            AttachFileToEmail(message, file);
        }
        await SendEmailAsync(message);
        return Result.Success();
    }

    private MailMessage CreateEmailMessage(string from, string to, string subject, string htmlBody)
    {
        var emailMessage = new MailMessage();
        emailMessage.From = new MailAddress(from);
        emailMessage.To.Add(new MailAddress(to));
        emailMessage.Subject = subject;
        emailMessage.IsBodyHtml = true;
        emailMessage.Body = htmlBody;

        return emailMessage;
    }

    private void AttachFileToEmail(MailMessage emailMessage, IFormFile file)
    {
        using var stream = file.OpenReadStream();
        var attachment = new Attachment(stream, file.FileName);
        emailMessage.Attachments.Add(attachment);
    }

    private async Task<Result> SendEmailAsync(MailMessage emailMessage)
    {

        try
        {
            using var smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
            {
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                EnableSsl = _smtpSettings.EnableSsl // Use SSL if enabled in configuration
            };
            await smtpClient.SendMailAsync(emailMessage);
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

