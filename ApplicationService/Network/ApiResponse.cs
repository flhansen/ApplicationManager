namespace ApplicationService.Network;

public class ApiResponse
{
    public ApiResponse(string message)
    {
        Message = message;
    }

    public string Message { get; set; } = string.Empty;
}