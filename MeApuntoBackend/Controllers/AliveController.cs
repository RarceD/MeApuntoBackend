using MeApuntoBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeApuntoBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AliveController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;
    public AliveController(ILogger<LoginController> logger)
    {
        _logger = logger;
    }
    [HttpGet]
    public string MakeLogin()
    {
        _logger.LogWarning("running..." + DateTime.Now.ToString());
        return DateTime.Now.ToString();
    }
}