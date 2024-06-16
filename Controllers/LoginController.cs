using CVR_API.Data;
using CVR_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CVR_API.Controllers;

//[Route("api/[controller]")]
//[ApiController]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly CVR_APIContext _context;
    private readonly ILogger<LoginController> _logger;

    public LoginController(IConfiguration config, CVR_APIContext context, ILogger<LoginController> logger)
    {
        _config = config;
        _context = context;
        _logger = logger;
    }

    public IActionResult Login([FromBody] LoginUser loginUser)
    {
        var user = AuthenticateUser(loginUser);

        if (user != null)
        {
            _logger.LogInformation("Authentication successful!");
            var tokenString = GenerateToken(user);
            var userObj = _context.User
                .Where(x => x.Name == user.UserName)
                .Single();

            return Ok(userObj);
        }

        return NotFound();
    }

    private LoginUser? AuthenticateUser(LoginUser loginUser)
    {
        bool isMatch = _context.User.Any(user => user.Name == loginUser.UserName);

        if (isMatch)
        {
            loginUser = new LoginUser { Id = Guid.NewGuid(), UserName = loginUser.UserName };
            return loginUser;
        }

        return null;
    }

    private string GenerateToken(LoginUser loginUser)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"] ?? throw new Exception("No Key was found")));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, loginUser.UserName)
        };

        var token = new JwtSecurityToken(_config["JWT:Issuer"],
            _config["JWT:Issuer"],
            claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}