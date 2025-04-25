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
    private bool _isSendEnabled;

    public string Sender
    {
        get => _sender;
        set
        {
            _sender = value;
            OnPropertyChanged();
            UpdateIsSendEnabled();
        }
    }

    public string Recipient
    {
        get => _recipient;
        set
        {
            _recipient = value;
            OnPropertyChanged();
            UpdateIsSendEnabled();
        }
    }

    public string Subject
    {
        get => _subject;
        set
        {
            _subject = value;
            OnPropertyChanged();
            UpdateIsSendEnabled();
        }
    }

    public string Body
    {
        get => _body;
        set
        {
            _body = value;
            OnPropertyChanged();
            UpdateIsSendEnabled();
        }
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

    public bool IsSendEnabled
    {
        get => _isSendEnabled;
        private set { _isSendEnabled = value; OnPropertyChanged(); }
    }

    public ICommand SendEmailCommand { get; }

    public EmailViewModel()
    {
        _user = App.GetService<User>();
        _emailService = App.GetService<EmailService>();
        SendEmailCommand = new AsyncRelayCommand<Task>(async _ =>
        {
            IsBusy = true;
            IsStatusVisible = false;
            var result = await SendEmailAsync(Sender, Recipient, Subject, Body);
            if (result.Status == CallStatus.Success)
            {
                StatusMessage = "Email sent successfully!";
                IsSuccess = true;
            }
            else
            {
                StatusMessage = result.Message;
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

    private void UpdateIsSendEnabled()
    {
        IsSendEnabled = !string.IsNullOrWhiteSpace(Sender) &&
                        !string.IsNullOrWhiteSpace(Recipient) &&
                        !string.IsNullOrWhiteSpace(Subject) &&
                        !string.IsNullOrWhiteSpace(Body);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
