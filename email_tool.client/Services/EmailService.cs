using System.Net.Http;
using System.Text;
using System.Text.Json;
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

    /// <summary>
    /// Sends an email asynchronously using the provided message and authentication token.
    /// </summary>
    /// <param name="message">The email message to be sent, including sender, recipient, subject, and body.</param>
    /// <param name="token">The authentication token used for authorization in the HTTP request.</param>
    /// <returns>
    /// A <see cref="CallResult{T}"/> object containing the status, message, and optional data (e.g., response data).
    /// </returns>
    public async Task<CallResult<string>> SendEmailAsync(MessageModel message, string token)
    {
        // Serialize the email message into JSON format and set the content type to application/json.
        var content = new StringContent(JsonSerializer.Serialize(message), Encoding.UTF8, "application/json");

        // Add the Bearer token to the HTTP request headers for authorization.
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // Send a POST request to the email API endpoint with the serialized message content.
        var response = await _httpClient.PostAsync("http://localhost:5000/api/email/send", content);

        
        var responseData = await response.Content.ReadAsStringAsync();
        var callRes = JsonSerializer.Deserialize<CallResult<string>>(responseData, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        //TODO: need to add better error handling around deserialization and callRes being potentially null
        
        // If the response indicates success, deserialize the response data and return it.
        if (response.IsSuccessStatusCode)
        {
            return callRes!;
        }

        // If the response indicates failure, return a failure result with an appropriate message.
        return new CallResult<string>
        {
            Status = CallStatus.Fail,
            Message = callRes!.Message
        };
    }
}
