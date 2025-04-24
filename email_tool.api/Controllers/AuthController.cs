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

    private string GenerateJwtToken(string username)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}


