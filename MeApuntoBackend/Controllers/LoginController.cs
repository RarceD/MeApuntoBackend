using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeApuntoBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;
    private readonly IClientManagementService _loginManagementService;
    public LoginController(ILogger<LoginController> logger, IClientManagementService loginManagementService)
    {
        _logger = logger;
        _loginManagementService = loginManagementService;
    }

    [HttpPost]
    public LoginResponse MakeLogin(LoginDto input)
    {
        if (input.User == null || input.Pass == null) return new() { Success = false };

        var response = _loginManagementService.CheckUserExist(input.User, input.Pass);
        if (!response.Success)
        {
            _logger.LogWarning("[LOGIN ERROR] User: " + input.User + " error login with pass " + input.Pass);
        }
        else
        {
            _logger.LogWarning("[LOGIN OK] User: " + input.User + " login with id " + response.Id);
        }
        return response;

    }
}