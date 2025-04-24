using System.Threading.Tasks;
using System.Windows.Input;
using email_tool.client.Commands;
using email_tool.client.Services;
using email_tool.shared.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using email_tool.client.Models;
using email_tool.shared.Enums;

namespace email_tool.client.ViewModels;

public class EmailViewModel : INotifyPropertyChanged
{
    private readonly EmailService? _emailService;
    private User? _user;
    private string _sender;
    private string _recipient;
    private string _subject;
    private string _body;
    private string _statusMessage;
    private bool _isStatusVisible;
    private bool _isSuccess;
    private bool _isBusy;

    public string Sender
    {
        get => _sender;
        set { _sender = value; OnPropertyChanged(); }
    }

    public string Recipient
    {
        get => _recipient;
        set { _recipient = value; OnPropertyChanged(); }
    }

    public string Subject
    {
        get => _subject;
        set { _subject = value; OnPropertyChanged(); }
    }

    public string Body
    {
        get => _body;
        set { _body = value; OnPropertyChanged(); }
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set { _statusMessage = value; OnPropertyChanged(); }
    }

    public bool IsStatusVisible
    {
        get => _isStatusVisible;
        set { _isStatusVisible = value; OnPropertyChanged(); }
    }

    public bool IsSuccess
    {
        get => _isSuccess;
        set { _isSuccess = value; OnPropertyChanged(); }
    }

    public bool IsBusy
    {
        get => _isBusy;
        set { _isBusy = value; OnPropertyChanged(); }
    }

    public ICommand SendEmailCommand { get; }

    public EmailViewModel()
    {
        _user = App.GetService<User>();
        _emailService = App.GetService<EmailService>();
        SendEmailCommand = new AsyncRelayCommand<Task>(async _ =>
        {
            IsBusy = true;
            var result = await SendEmailAsync(Sender, Recipient, Subject, Body);
            if (result.Status == CallStatus.Success)
            {
                StatusMessage = "Email sent successfully!";
                IsSuccess = true;
            }
            else
            {
                StatusMessage = $"Failed to send email: {result.Message}";
                IsSuccess = false;
            }
            IsStatusVisible = true;
            IsBusy = false;
        });
    }

    public async Task<CallResult<string>> SendEmailAsync(string sender, string recipient, string subject, string body)
    {
        var message = new MessageModel(sender, recipient, subject, body);
        return await _emailService.SendEmailAsync(message, _user.Token);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
