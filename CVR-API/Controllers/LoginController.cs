using CVR_API.Data;
using CVR_API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CVR_API.Controllers;

[Route("api/[controller]")]
[EnableRateLimiting("fixed")]
[EnableCors("MyPolicy")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly CVR_APIContext _context;
    private readonly ILogger<LoginController> _logger;
    private readonly IOptions<JwtBearerOptions> _jwtOptions;

    public LoginController(IConfiguration config, CVR_APIContext context, ILogger<LoginController> logger, IOptions<JwtBearerOptions> jwtOptions)
    {
        _config = config;
        _context = context;
        _logger = logger;
        _jwtOptions = jwtOptions;
    }

    private User? FindExistingUser(LoginUser loginUser)
    {
        try
        {
            return _context.User
                .Where(x => x.Name == loginUser.UserName && x.Password == loginUser.Password)
                .Single();
        }

        catch (Exception ex) 
        {
            _logger.LogError($"${ex.Message}");
            return null;
        }  
    }

    /// <summary>
    /// Logs in a user
    /// </summary>
    /// <remarks>
    /// Example request:
    /// 
    ///     POST /login
    ///     {
    ///         "username": "A Username",
    ///         "password": "A Password"
    ///     }
    /// 
    /// </remarks>>
    /// <response code="200">Returns OK with a Web Token if a user has already been created with the given name</response>
    /// <response code="404">Returns NotFound if a user with the given name doesn't exist</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginUser loginUser)
    {
        var user = AuthenticateUser(loginUser);

        if (user != null)
        {
            var tokenString = GenerateToken(user
                              ?? throw new NullReferenceException("User is null"));

            var userobj = FindExistingUser(loginUser);
            if (userobj != null) return Ok(tokenString);
        }

        return NotFound($"No user exists with the name: {loginUser.UserName} and the password: {loginUser.Password}");
    }

    private bool IsMissingOrIsNotBearer(string headerValue)
    {
        return string.IsNullOrEmpty(headerValue) || !headerValue.StartsWith("Bearer");
    }

    /// <summary>
    ///  Gets the full information about a user if the attached token is valid
    /// </summary>
    /// <response code="200">Returns OK with the full user info if they are authorized with a JSON web Token</response>
    /// <response code="401">Returns Unauthorized if the user does not provide a token in the header</response>>
    [HttpPost("/full")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    public IActionResult fullUserDetails([FromHeader] string authorizationHeaderValue, [FromBody] LoginUser loginUser)
    {
        if (IsMissingOrIsNotBearer(authorizationHeaderValue))
        {
            return Unauthorized("Token is missing or is invalid!");
        }

        var token = authorizationHeaderValue.Substring("Bearer ".Length).Trim();
        _logger.LogInformation($"Token value: {token}");
        var validationParameters = _jwtOptions.Value.TokenValidationParameters;
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var validationResult = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken result);
            return validationResult != null ? Ok(FindExistingUser(loginUser)) : Unauthorized("Token validation failed!");
        }

        catch (SecurityTokenException e)
        {
            _logger.LogError($"Error with token: {e}");
            return BadRequest();
        }

        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex}");
            return BadRequest();
        }
    }

    private LoginUser? AuthenticateUser(LoginUser loginUser)
    {
        bool isMatch = _context.User.Any(user => user.Name == loginUser.UserName);

        if (isMatch)
        {
            loginUser = new LoginUser(loginUser.UserName,loginUser.Password);
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

        var token = new JwtSecurityToken(issuer: _config["JWT:Issuer"],
            audience: _config["JWT:Audience"],
            claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}