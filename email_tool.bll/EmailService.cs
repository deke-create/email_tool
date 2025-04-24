using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using email_tool.shared.Enums;
using email_tool.shared.Models;
using Microsoft.Extensions.Logging;

namespace email_tool.bll;

public interface IEmailService
{
    Task<CallResult<string>> SendMessage(MessageModel message);
}

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly SmtpClient _smtpClient;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, SmtpClient smtpClient, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _smtpClient = smtpClient;
        _logger = logger;

        var smtpSettings = _configuration.GetSection("SmtpSettings");
        _smtpClient.Host = smtpSettings["Host"];
        _smtpClient.Port = int.Parse(smtpSettings["Port"] ?? string.Empty);
        _smtpClient.Credentials = new System.Net.NetworkCredential(
            smtpSettings["Username"],
            smtpSettings["Password"]
        );
        _smtpClient.EnableSsl = bool.Parse(smtpSettings["EnableSsl"] ?? string.Empty);
    }

    /// <summary>
    /// Sends an email message with retry logic in case of failures.
    /// </summary>
    /// <param name="message">The email message to be sent, containing sender, recipient, subject, and body.</param>
    /// <returns>
    /// A <see cref="CallResult{T}"/> object containing the status, message, and optional data (e.g., message ID).
    /// </returns>
    public async Task<CallResult<string>> SendMessage(MessageModel message)
    {
        // Maximum number of retry attempts for sending the email.
        const int maxRetries = 3;

        // Current attempt counter.
        var attempt = 0;

        // Delay between retries, in milliseconds, retrieved from configuration.
        var retryDelay = int.Parse(_configuration["RetryDelayMilliseconds"] ?? "1000");

        // Retry loop for sending the email.
        while (attempt < maxRetries)
        {
            attempt++;
            try
            {
                // Log the current attempt to send the email.
                _logger.LogInformation("Attempt {Attempt} to send email to {Recipient}", attempt, message.Recipient);

                // Create the email message with the provided details.
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(message.Sender),
                    Subject = message.Subject,
                    Body = message.Body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(message.Recipient);

                // Attempt to send the email asynchronously.
                await _smtpClient.SendMailAsync(mailMessage);

                // Log success and return a successful result.
                _logger.LogInformation("Email successfully sent to {Recipient}", message.Recipient);

                return new CallResult<string>
                {
                    Status = CallStatus.Success,
                    Message = $"Email sent to {message.Recipient}",
                    Data = "MailMessageId3030"
                };
            }
            catch (Exception ex)
            {
                // Log the error for the current attempt.
                _logger.LogError(ex, "Failed to send email to {Recipient} on attempt {Attempt}", message.Recipient, attempt);

                // If maximum retries are reached, return a failure result.
                if (attempt >= maxRetries)
                {
                    return new CallResult<string>
                    {
                        Status = CallStatus.Fail,
                        Message = $"Failed to send email to {message.Recipient} after {maxRetries} attempts. Error: {ex.Message}",
                        Data = null
                    };
                }

                // Wait for the configured delay before retrying.
                await Task.Delay(retryDelay);
            }
        }

        // Return a failure result if the loop exits unexpectedly.
        return new CallResult<string>
        {
            Status = CallStatus.Fail,
            Message = "Unexpected error occurred.",
            Data = null
        };
    }
}
