using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeApuntoBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatsController : GenericController
{
    private readonly IStatsService _statsService;
    private readonly IBookerManagementService _bookerManagementService;
    public StatsController(ILogger<StatsController> logger, 
        IClientManagementService loginManagementService,
        IBookerManagementService bookerManagementService,
        IStatsService statsService)
        : base(loginManagementService)
    {
        _bookerManagementService = bookerManagementService;
        _statsService = statsService;
    }

    [HttpPost]
    public ActionResult UpdateProfileInfo(StatsDto stats)
    {
        if (!CheckUserTokenId(stats.Token ?? string.Empty, stats.Id)) return NoContent();
        _statsService.AddLoginRecord(new() { Success = true, RegisterTime = DateTime.Now, AutoLogin = true });
        return Ok();
    }

    [HttpGet]
    public IActionResult GetStats(string token, int id)
    {
        if (!IsAdmin(id, token)) return GetNotAdminResponse();
        return Ok(_clientManagementService.GetStats());
    }

    [HttpGet("booker")]
    public IActionResult GetBooksStats(string token, int id)
    {
        if (!IsAdmin(id, token)) return GetNotAdminResponse();
        return Ok(_bookerManagementService.GetStats());
    }
}