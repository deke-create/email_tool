using System.Threading.Tasks;
using System.Windows.Input;
using email_tool.client.Commands;
using email_tool.client.Services;
using email_tool.shared.Models;

namespace email_tool.client.ViewModels;

public class EmailViewModel
{
    private readonly EmailService _emailService;

    public ICommand SendEmailCommand { get; }

    public EmailViewModel(EmailService emailService)
    {
        _emailService = emailService;
        SendEmailCommand = new AsyncRelayCommand<object>(async (parameter) =>
        {
            if (parameter is (string sender, string recipient, string subject, string body))
            {
                await SendEmailAsync(sender, recipient, subject, body);
            }
        });
    }

    public async Task<CallResult<string>> SendEmailAsync(string sender, string recipient, string subject, string body)
    {
        var message = new MessageModel(sender, recipient, subject, body);
        return await _emailService.SendEmailAsync(message);
    }
}
