using email_tool.bll;
using email_tool.shared.Enums;
using email_tool.shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailController : Controller
{
    private readonly ILogger<EmailController> _logger;
    private readonly IEmailService _emailService;

    public EmailController(ILogger<EmailController> logger, IEmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }

    [Route("/send"), HttpPost]
    public async Task<IActionResult> Send(MessageModel message)
    {
        if (message == null)
        {
            return BadRequest("Message cannot be null");
        }

        if (string.IsNullOrEmpty(message.Sender) || string.IsNullOrEmpty(message.Recipient))
        {
            return BadRequest("Sender and Recipient cannot be empty");
        }

        var result = await _emailService.SendMessage(message);

        if (result.Status == CallStatus.Success)
        {
            return Ok(result);
        }

        return StatusCode(500, result);
    }
}
