using System.Threading.Tasks;
using email_tool.client.Services;
using email_tool.shared.Models;

namespace email_tool.client.ViewModels;

public class AuthViewModel
{
    private readonly AuthService _authService;

    public AuthViewModel(AuthService authService)
    {
        _authService = authService;
    }

    public async Task<CallResult<string>> LoginAsync(string username, string password)
    {
        var loginRequest = new LoginRequestModel
        {
            Username = username,
            Password = password
        };

        return await _authService.LoginAsync(loginRequest);
    }
}
