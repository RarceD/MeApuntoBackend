using Microsoft.AspNetCore.Mvc;

namespace MeApuntoBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AliveController : ControllerBase
{
    [HttpGet]
    public string MakeLogin()
    {
        return DateTime.Now.ToString();
    }
}