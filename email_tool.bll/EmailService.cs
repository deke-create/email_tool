using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using email_tool.shared.Enums;
using email_tool.shared.Models;

namespace email_tool.bll;

public interface IEmailService
{
    Task<CallResult<string>> SendMessage(MessageModel message);
}

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly SmtpClient _smtpClient;

    public EmailService(IConfiguration configuration, SmtpClient smtpClient)
    {
        _configuration = configuration;
        _smtpClient = smtpClient;

        var smtpSettings = _configuration.GetSection("SmtpSettings");
        _smtpClient.Host = smtpSettings["Host"];
        _smtpClient.Port = int.Parse(smtpSettings["Port"] ?? string.Empty);
        _smtpClient.Credentials = new System.Net.NetworkCredential(
            smtpSettings["Username"], 
            smtpSettings["Password"]
        );
        _smtpClient.EnableSsl = bool.Parse(smtpSettings["EnableSsl"] ?? string.Empty);
    }

    public async Task<CallResult<string>> SendMessage(MessageModel message)
    {
        const int maxRetries = 3;
        var attempt = 0;

        while (attempt < maxRetries)
        {
            attempt++;
            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(message.Sender),
                    Subject = message.Subject,
                    Body = message.Body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(message.Recipient);

                await _smtpClient.SendMailAsync(mailMessage);

                return new CallResult<string>
                {
                    Status = CallStatus.Success,
                    Message = $"Email sent to {message.Recipient}",
                    Data = "MailMessageId3030" // Simulated email ID
                };
            }
            catch (Exception ex)
            {
                if (attempt >= maxRetries)
                {
                    return new CallResult<string>
                    {
                        Status = CallStatus.Fail,
                        Message = $"Failed to send email to {message.Recipient} after {maxRetries} attempts. Error: {ex.Message}",
                        Data = null
                    };
                }
                await Task.Delay(1000); // Wait 1 second before retrying
            }
        }

        return new CallResult<string>
        {
            Status = CallStatus.Fail,
            Message = "Unexpected error occurred.",
            Data = null
        };
    }
}
