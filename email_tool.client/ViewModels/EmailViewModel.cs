using System.Threading.Tasks;
using email_tool.client.Services;
using email_tool.shared.Models;

namespace email_tool.client.ViewModels;

public class EmailViewModel
{
    private readonly EmailService _emailService;

    public EmailViewModel(EmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task<CallResult<string>> SendEmailAsync(string sender, string recipient, string subject, string body)
    {
        var message = new MessageModel(sender, recipient, subject, body);
        return await _emailService.SendEmailAsync(message);
    }
}
