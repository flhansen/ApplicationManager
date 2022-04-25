using ApplicationService.Models;

namespace ApplicationService.Network;

public class ApplicationsResponse : ApiResponse
{
    public ApplicationsResponse(string message, IEnumerable<Application> applications) : base(message)
    {
        Applications = applications;
    }

    public IEnumerable<Application> Applications { get; set; } = new List<Application>();
}