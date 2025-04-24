using email_tool.shared.Enums;

namespace email_tool.shared.Models;

public class CallResult<T>
{
    public CallStatus Status { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
}