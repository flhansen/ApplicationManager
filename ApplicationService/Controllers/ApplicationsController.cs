using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using ApplicationService.Models;
using ApplicationService.Network;
using System.Security.Claims;

namespace ApplicationService.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ApplicationsController : Controller
{
    private readonly ApplicationDataContext _context;

    public ApplicationsController(ApplicationDataContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetApplications()
    {
        var userIdIsValid = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId);

        if (!userIdIsValid)
            return Unauthorized();

        var applications =
            from a in _context.Applications
            join w in _context.WorkTypes!
                on a.WorkType equals w.Id
            join s in _context.Statuses!
                on a.Status equals s.Id
            where a.UserId == userId
            select a;


        return Ok(new ApplicationsResponse("Applications were found", await applications.ToListAsync()));
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse>> GetApplication(int id)
    {
        var application = await (
            from a in _context.Applications
            join w in _context.WorkTypes!
                on a.WorkType equals w.Id
            join s in _context.Statuses!
                on a.Status equals s.Id
            where a.Id == id
            select a).FirstAsync();

        if (application == null)
            return NotFound();

        return new ApplicationResponse("Application found", application);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreateApplication(Application application)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return BadRequest(new ApiResponse("User ID is missing"));

        application.UserId = userId;
        await _context.Applications.AddAsync(application);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            return BadRequest(new ApiResponse(ex.Message));
        }

        return Ok(new ApiResponse("Application created"));
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteApplication(int id)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return BadRequest(new ApiResponse("An error occured"));

        var app =
            await (
                from a in _context.Applications
                where a.Id == id && a.UserId == userId
                select a
            ).FirstOrDefaultAsync();

        if (app == null)
            return BadRequest(new ApiResponse("The application does not exist"));

        _context.Applications.Remove(app);
        _context.SaveChanges();

        return Ok(new ApiResponse("Application deleted"));
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<ApiResponse>> UpdateApplication(Application application)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            return BadRequest(new ApiResponse("An error occured"));

        var app =
            await (
                from a in _context.Applications
                where a.UserId == userId
                select a
            ).FirstOrDefaultAsync();

        if (app == null)
            return NotFound(new ApiResponse("The application does not exist"));

        app.Status = application.Status;
        app.StartDate = application.StartDate;
        app.SubmissionDate = application.SubmissionDate;
        app.StartDate = application.StartDate;
        app.Commentary = application.Commentary;
        app.AcceptedSalary = application.AcceptedSalary;
        app.CompanyName = application.CompanyName;
        app.WantedSalary = application.WantedSalary;
        _context.SaveChanges();

        return Ok();
    }
}