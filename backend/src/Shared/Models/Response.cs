namespace Shared.Models;

public class Response<T>
{
    public string IsSuccessful { get; set; } = string.Empty;
    public T? Payload { get; set; } = default;
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
}
