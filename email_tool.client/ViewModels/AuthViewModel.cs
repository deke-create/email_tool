using System.Threading.Tasks;
using System.Windows.Input;
using email_tool.client.Commands;
using email_tool.client.Services;
using email_tool.shared.Models;

namespace email_tool.client.ViewModels;

public class AuthViewModel
{
    private readonly AuthService _authService;

    public ICommand LoginCommand { get; }

    public AuthViewModel(AuthService authService)
    {
        _authService = authService;
        LoginCommand = new AsyncRelayCommand<object>(async (parameter) =>
        {
            if (parameter is (string username, string password))
            {
                await LoginAsync(username, password);
            }
        });
    }

    public async Task<CallResult<string>> LoginAsync(string username, string password)
    {
        var loginRequest = new LoginRequestModel
        {
            Username = username,
            Password = password
        };

        var res = await _authService.LoginAsync(loginRequest);
        return res;
    }
}
