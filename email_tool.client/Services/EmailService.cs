using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using email_tool.shared.Enums;
using email_tool.shared.Models;

namespace email_tool.client.Services;

public class EmailService
{
    private readonly HttpClient _httpClient;

    public EmailService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CallResult<string>> SendEmailAsync(MessageModel message)
    {
        var content = new StringContent(JsonSerializer.Serialize(message), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("http://localhost:5000/api/email/send", content);

        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CallResult<string>>(responseData);
        }

        return new CallResult<string>
        {
            Status = CallStatus.Fail,
            Message = "Failed to send email"
        };
    }
}
