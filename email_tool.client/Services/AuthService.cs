using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using email_tool.shared.Enums;
using email_tool.shared.Models;

namespace email_tool.client.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CallResult<string>> LoginAsync(LoginRequestModel loginRequest)
    {

        try
        {
            var content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5000/api/auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<CallResult<string>>(responseData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return res!;
            }

            return new CallResult<string>
            {
                Status = CallStatus.Fail,
                Message = "Login failed"
            };
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during login: {ex.Message}");
            return new CallResult<string>
            {
                Status = CallStatus.Error,
                Message = "An error occurred during login. Please try again later."
            };
        }


    }
}
