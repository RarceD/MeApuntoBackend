using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace MeApuntoBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CreateController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;
    private readonly IClientManagementService _loginManagementService;
    public CreateController(ILogger<LoginController> logger, IClientManagementService loginManagementService)
    {
        _logger = logger;
        _loginManagementService = loginManagementService;
    }

    //[HttpGet("{id}")]
    [HttpPost]
    public ActionResult CreateUser(CreateDto input)
    {
        var resp = NoContent();

        // Apply filter to mandatory params
        if (input.Pass == null || input.Name == null || input.Key == null) return resp;

        // Apply filter to mail address:
        if (input.User == null) return resp;
        input.User = input.User.ToLower().Replace(" ", "").ToString();

        // Validate urba key:
        var urbaId = _loginManagementService.GetValidUrbaKeyId(input.Key);
        if (urbaId == 0) return resp;

        // Decode code to base64 - input.Name
        var base64EncodedBytes = Convert.FromBase64String(input.Name);
        string code = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        input.Name = code;
        // Validate code from user, maybe is in use
        if (!_loginManagementService.IsValidUserCode(code)) return resp;

        // Create the user:
        var success = _loginManagementService.AddClient(input, urbaId);
        if (success) return Ok();
        return resp;
    }
}