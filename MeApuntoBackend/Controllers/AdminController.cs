using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeApuntoBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : GenericController
{
    public AdminController(IClientManagementService clientManagementService) : base(clientManagementService)
    {
    }

    [HttpGet("code")]
    public IActionResult GetMatchCode(string token, int id, string matchStr)
    {
        if (!IsAdmin(id, token)) return GetNotAdminResponse();
        return Ok(_clientManagementService.GetCodeContains(matchStr.ToLower()));
    }

    [HttpGet("email")]
    public IActionResult GetMatchEmail(string token, int id, string matchStr)
    {
        if (!IsAdmin(id, token)) return GetNotAdminResponse();
        return Ok(_clientManagementService.GetEmailContains(matchStr.ToLower()));
    }
}