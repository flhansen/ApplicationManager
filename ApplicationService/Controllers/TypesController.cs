using ApplicationService.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationService.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class TypesController : Controller
{
    private readonly ApplicationDataContext _context;

    public TypesController(ApplicationDataContext context)
    {
        _context = context;
    }

    [HttpGet("worktypes")]
    public ActionResult<IEnumerable<WorkType>> GetWorkTypes()
    {
        var workTypes = 
            from a in _context.WorkTypes
            select a;

        return Ok(workTypes);
    }

    [HttpGet("statuses")]
    public ActionResult<IEnumerable<ApplicationStatus>> GetApplicationStatuses()
    {
        var statuses =
            from a in _context.Statuses
            select a;

        return Ok(statuses);
    }
}