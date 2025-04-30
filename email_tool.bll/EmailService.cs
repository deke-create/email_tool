using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using email_tool.shared.Enums;
using email_tool.shared.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace email_tool.bll;

public interface IEmailService
{
    Task<CallResult<string>> SendMessage(MessageModel message);
    Task RetryFailedMessages();
}

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly SmtpClient _smtpClient;
    private readonly ILogger<EmailService> _logger;
    private static readonly ConcurrentQueue<MessageModel> _failedMessagesQueue = new();

    public EmailService(IConfiguration configuration, SmtpClient smtpClient, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _smtpClient = smtpClient;
        _logger = logger;

        var smtpSettings = _configuration.GetSection("SmtpSettings");
        _smtpClient.Host = smtpSettings["Host"] ?? string.Empty;
        _smtpClient.Port = int.Parse(smtpSettings["Port"] ?? string.Empty);
        _smtpClient.Credentials = new System.Net.NetworkCredential(
            smtpSettings["Username"],
            smtpSettings["Password"]
        );
        _smtpClient.EnableSsl = bool.Parse(smtpSettings["EnableSsl"] ?? string.Empty);
    }


    /// <summary>
    /// Sends an email message to a specified recipient.
    /// </summary>
    /// <param name="message">The email message to be sent, including sender, recipient, subject, and body details.</param>
    /// <returns>A <see cref="CallResult{T}"/> containing the status, a message, and optional data related to the operation.</returns>
    public async Task<CallResult<string>> SendMessage(MessageModel message)
    {
        const int maxRetries = 3;
        var attempt = 0;
        var retryDelay = int.Parse(_configuration["RetryDelayMilliseconds"] ?? "1000");

        while (attempt < maxRetries)
        {
            attempt++;
            try
            {
                _logger.LogInformation("Attempt {Attempt} to send email to {Recipient}", attempt, message.Recipient);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(message.Sender),
                    Subject = message.Subject,
                    Body = message.Body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(message.Recipient);

                await _smtpClient.SendMailAsync(mailMessage);

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
                _logger.LogError(ex, "Failed to send email to {Recipient} on attempt {Attempt}", message.Recipient, attempt);

                if (attempt == 1)
                {
                    // Notify the user that retries will be attempted.
                    return new CallResult<string>
                    {
                        Status = CallStatus.Fail,
                        Message = $"First attempt to send email to {message.Recipient} failed. We will retry up to {maxRetries} times. Please check back soon.",
                        Data = null
                    };
                }

                if (attempt >= maxRetries)
                {
                    _failedMessagesQueue.Enqueue(message); // Add to queue for later retry.
                    return new CallResult<string>
                    {
                        Status = CallStatus.Fail,
                        Message = $"Failed to send email to {message.Recipient} after {maxRetries} attempts. Error: {ex.Message}",
                        Data = null
                    };
                }

                await Task.Delay(retryDelay);
            }
        }

        return new CallResult<string>
        {
            Status = CallStatus.Fail,
            Message = "Unexpected error occurred.",
            Data = null
        };
    }

    /// <summary>
    /// Processes the failed messages queue and retries sending them.
    /// </summary>
    public async Task RetryFailedMessages()
    {
        while (_failedMessagesQueue.TryDequeue(out var message))
        {
            _logger.LogInformation("Retrying email to {Recipient}", message.Recipient);
            var result = await SendMessage(message);

            if (result.Status == CallStatus.Fail)
            {
                _logger.LogWarning("Retry failed for email to {Recipient}. Adding back to queue.", message.Recipient);
                _failedMessagesQueue.Enqueue(message); // Re-add to queue if retry fails.
            }
        }
    }
}
