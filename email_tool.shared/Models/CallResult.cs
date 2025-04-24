using email_tool.shared.Enums;

namespace email_tool.shared.Models;

public class CallResult
{
    public CallStatus Status { get; set; }
    public string Message { get; set; } = string.Empty;
}