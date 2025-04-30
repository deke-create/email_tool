using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using email_tool.shared.Enums;
using email_tool.shared.Models;

namespace email_tool.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IConfiguration configuration, ILogger<AuthController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost("login")]
    public ActionResult<CallResult<string>> Login([FromBody] LoginRequestModel requestModel)
    {
        var configuredUsername = _configuration["Auth:Username"];
        var configuredPassword = _configuration["Auth:Password"];

        if (configuredUsername == requestModel.Username && configuredPassword == requestModel.Password)
        {
            var token = GenerateJwtToken(requestModel.Username);
            var res = new CallResult<string>
            {
                Status = CallStatus.Success,
                Message = "Login successful",
                Data = token
            };
            return Ok(res);
        }

        _logger.LogWarning("Invalid login attempt for user {Username}", requestModel.Username);
        return Unauthorized(new CallResult<string>
        {
            Status = CallStatus.Fail,
            Message = "Invalid username or password"
        });
    }

    /// <summary>
    /// Generates a JSON Web Token (JWT) for the specified username.
    /// </summary>
    /// <param name="username">The username for which the token is being generated.</param>
    /// <returns>A signed JWT as a string.</returns>
    private string GenerateJwtToken(string username)
    {
        // Create a symmetric security key using the secret key from configuration.
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
    
        // Create signing credentials using the security key and HMAC-SHA256 algorithm.
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Define the claims for the token, including the subject (username) and a unique identifier (JTI).
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username), // Subject claim with the username.
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique identifier for the token.
        };

        // Create the JWT with the specified issuer, audience, claims, expiration, and signing credentials.
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"], // Token issuer from configuration.
            audience: _configuration["Jwt:Audience"], // Token audience from configuration.
            claims: claims, // Claims defined above.
            expires: DateTime.UtcNow.AddHours(1), // Token expiration set to 1 hour from now.
            signingCredentials: creds // Signing credentials for the token.
        );

        // Serialize the token to a string and return it.
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}


