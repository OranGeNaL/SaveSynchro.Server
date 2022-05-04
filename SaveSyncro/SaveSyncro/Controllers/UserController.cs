using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SaveSyncro.Contexts;
using SaveSyncro.Models;

namespace SaveSyncro.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private UserContext db;

    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger, UserContext context)
    {
        _logger = logger;
        db = context;
    }

    [HttpPost("register")]
    public async Task<ActionResult<IResult>> RegisterNewUser(User newUser)
    {
        if (db.Users.FirstOrDefault(x => x.Login == newUser.Login) != null)
            return NotFound();

        User user = new User(newUser.Login, newUser.Password, Guid.NewGuid().ToString());
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, newUser.Login),
            new Claim("UserID", user.UserID)
        };
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.Issuer,
            audience: AuthOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromDays(10)), // время действия 2 минуты
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        
        var encodedJWT = new JwtSecurityTokenHandler().WriteToken(jwt);
        
        db.Users.Add(user);
        await db.SaveChangesAsync();
        
        var response = new LoginResponse()
        {
            AccessToken = encodedJWT,
            Login = user.Login
        };

        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<User>> GetUserInfo()
    {
        User user = await db.Users.FirstOrDefaultAsync(x => x.Login == HttpContext.User.Identity.Name);
        if (user == null)
            return NotFound();
            
        return user;
    }
}