using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeApuntoBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ForgetController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;
    private readonly IClientManagementService _clientManagementService;
    public ForgetController(ILogger<LoginController> logger, IClientManagementService loginManagementService)
    {
        _logger = logger;
        _clientManagementService = loginManagementService;
    }

    [HttpPost]
    public ForgetResponse ForgetFuckingPass(ForgetDto input)
    {
        var resp = new ForgetResponse() {Success = false };
        if (input.Username == null) return resp;

        resp.Success = _clientManagementService.ForgetPassword(input.Username);
        if (resp.Success)
            _logger.LogWarning("Forget password user: " + input.Username + " OK");
        else
            _logger.LogError("Forget password user: " + input.Username + " does not exist in DB");
        return resp;
    }
}