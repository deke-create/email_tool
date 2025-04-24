using email_tool.shared.Enums;
using email_tool.shared.Models;

namespace email_tool.bll;

public interface IEmailService
{
    Task<CallResult<string>> SendMessage(MessageModel message);
}

public class EmailService : IEmailService
{
    public async Task<CallResult<string>> SendMessage(MessageModel message)
    {
        const int maxRetries = 3;
        var attempt = 0;

        while (attempt < maxRetries)
        {
            attempt++;
            try
            {
                // Simulate async email sending logic
                await Task.Delay(100); // Simulate a delay for async operation
                // Simulate success
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

        // This point should not be reached
        return new CallResult<string>
        {
            Status = CallStatus.Fail,
            Message = "Unexpected error occurred.",
            Data = null
        };
    }
}
