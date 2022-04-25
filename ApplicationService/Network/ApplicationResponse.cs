using ApplicationService.Models;

namespace ApplicationService.Network;

public class ApplicationResponse : ApiResponse
{
    public ApplicationResponse(string message, Application application) : base(message)
    {
        Application = application;
    }

    public Application Application { get; set; }
}