using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services;
using Microsoft.AspNetCore.Mvc;

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
        if (urbaId == 0)
        {
            _logger.LogWarning("[CREATE] ERROR invalid urbaId code user: " + input.User + " with key: " + input.Key);
            return resp;
        }

        // Decode code to base64 - input.Name
        var base64EncodedBytes = Convert.FromBase64String(input.Name);
        string code = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        input.Name = code;
        // Validate code from user, maybe is in use
        if (!_loginManagementService.IsValidUserCode(code))
        {
            _logger.LogWarning("[CREATE] invalid user code user: " + input.User + " with code: " + code);
            return resp;
        }

        // Create the user:
        var success = _loginManagementService.AddClient(input, urbaId);
        if (success)
        {
            _logger.LogWarning("[CREATE] NEW user: " + input.User + " with code: " + code);
        }
        else
        {
            _logger.LogWarning("[CREATE] ERROR NEW user: " + input.User + " with code: " + code);
        }
        return success ? Ok() : resp;
    }

    [HttpPost("redirect")]
    public RedirectResponse SendCorrectUrl(RedirectDto input)
    {
        if (string.IsNullOrEmpty(input.Code)) return new RedirectResponse();
        try
        {
            var base64EncodedBytes = System.Convert.FromBase64String(input.Code);
            string code = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

            _logger.LogWarning("[CREATE] with code: " + input.Code);

            // verify other client does not have it:
            if (!_loginManagementService.IsValidUserCode(code)) return new();

            return new()
            {
                Success = true,
                Url = _loginManagementService.GenerateUrlForCode(code)
            };
        }
        catch (Exception ex)
        {
            _logger.LogWarning("[CREATE] ERROR with code: " + input.Code + " msg: " + ex.Message);
            return new();
        }
    }

}