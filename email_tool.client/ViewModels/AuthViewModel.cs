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

public class AuthViewModel : INotifyPropertyChanged
{
    private readonly AuthService? _authService;
    private User? _user;
    private string _username;
    private string _password;
    private string _errorMessage;
    private bool _isErrorVisible;

    public string Username
    {
        get => _username;
        set { _username = value; OnPropertyChanged(); }
    }

    public string Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(); }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set { _errorMessage = value; OnPropertyChanged(); }
    }

    public bool IsErrorVisible
    {
        get => _isErrorVisible;
        set { _isErrorVisible = value; OnPropertyChanged(); }
    }
    
    private bool _isBusy;

    public bool IsBusy
    {
        get => _isBusy;
        set { _isBusy = value; OnPropertyChanged(); }
    }

    public ICommand LoginCommand { get; }

    public AuthViewModel()
    {
        _authService = App.GetService<AuthService>();
        _user = App.GetService<User>();

        LoginCommand = new AsyncRelayCommand<object>(async _ =>
        {
            IsErrorVisible = false;
            IsBusy = true;
            var result = await LoginAsync(Username, Password);
            if (result.Status != CallStatus.Success)
            {
                ErrorMessage = result.Message;
                IsErrorVisible = true;
            }
            else
            {
                _user!.Token = result.Data!;
                _user.UserName = Username;
                App.NavigateToSendMessagePage();
            }
            IsBusy = false;
        });
    }

    public async Task<CallResult<string>> LoginAsync(string username, string password)
    {
        var loginRequest = new LoginRequestModel
        {
            Username = username,
            Password = password
        };

        var res = await _authService!.LoginAsync(loginRequest);
        return res;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
