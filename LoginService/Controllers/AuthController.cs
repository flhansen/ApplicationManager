using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using LoginService.Crypto;
using LoginService.Models;
using LoginService.Network;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace LoginService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly IConfiguration _config;
    private readonly LoginDataContext _context;

    public AuthController(LoginDataContext context, IConfiguration config)
    {
        _config = config;
        _context = context;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserAuthResponse>> Authenticate([FromBody] UserAuthRequest request)
    {
        var users =
            from u in _context.Users
            where u.UserName == request.UserName
            select u;
        var user = await users.FirstOrDefaultAsync();

        if (user == null || !Hasher.ValidateHash(user.Password!, request.Password!))
            return Unauthorized();

        var jwtSignKey = _config["JWT:SignKey"];
        if (jwtSignKey == null)
            return Problem();

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSignKey));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, request.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature),
            Audience = "ApplicationManager",
            Issuer = "https://localhost:7043/"
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return Ok(new UserAuthResponse
        {
            Token = tokenHandler.WriteToken(token)
        });
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser([FromBody] UserRegisterRequest request)
    {
        string passwordHash = Hasher.HMACSHA256Salted(request.Password, Hasher.GenerateSalt());
        await _context.Users.AddAsync(new User
        {
            UserName = request.UserName,
            Password = passwordHash,
            Email = request.Email,
            CreationDate = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
        return Ok();
    }

    [Authorize]
    [HttpDelete("delete")]
    public async Task<ActionResult> DeleteUser()
    {
        var username = User.FindFirstValue(ClaimTypes.Name);
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);

        if (user != null)
            _context.Users.Remove(user);

        return Ok();
    }
}