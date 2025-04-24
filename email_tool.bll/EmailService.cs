using email_tool.shared.Enums;
using email_tool.shared.Models;

namespace email_tool.bll;

public interface IEmailService
{
    Task<CallResult> SendMessage(MessageModel message);
}

public class EmailService : IEmailService
{
    public async Task<CallResult> SendMessage(MessageModel message)
    {
        // Simulate async email sending logic
        await Task.Delay(100); // Simulate a delay for async operation
        return new CallResult
        {
            Status = CallStatus.Success,
            Message = $"Email sent to {message.Recipient}"
        };
    }
}
