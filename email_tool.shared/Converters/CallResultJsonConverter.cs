using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using email_tool.shared.Enums;
using email_tool.shared.Models;

public class CallResultJsonConverter<T> : JsonConverter<CallResult<T>>
{
    public override CallResult<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonDocument = JsonDocument.ParseValue(ref reader);
        var rootElement = jsonDocument.RootElement;

        var status = rootElement.GetProperty("Status").GetInt32();
        var message = rootElement.GetProperty("Message").GetString();
        var data = rootElement.TryGetProperty("Data", out var dataElement) && dataElement.ValueKind != JsonValueKind.Null
            ? JsonSerializer.Deserialize<T>(dataElement.GetRawText(), options)
            : default;

        return new CallResult<T>
        {
            Status = (CallStatus)status,
            Message = message ?? string.Empty,
            Data = data
        };
    }

    public override void Write(Utf8JsonWriter writer, CallResult<T> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("Status", (int)value.Status);
        writer.WriteString("Message", value.Message);
        writer.WritePropertyName("Data");
        JsonSerializer.Serialize(writer, value.Data, options);
        writer.WriteEndObject();
    }
}