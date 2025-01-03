namespace Clinic.Core.Models.Response;

public class Response
{
    public dynamic? Data { get; set; }
    public required string Message { get; set; }
    public required bool Success { get; set; }
}
