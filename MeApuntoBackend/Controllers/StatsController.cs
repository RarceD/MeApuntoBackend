using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeApuntoBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatsController : GenericController
{
    private readonly ILogger<StatsController> _logger;
    private readonly IStatsService _statsService;
    public StatsController(ILogger<StatsController> logger, IClientManagementService loginManagementService, IStatsService statsService)
        : base(loginManagementService)
    {
        _logger = logger;
        _statsService = statsService;
    }

    [HttpPost]
    public ActionResult UpdateProfileInfo(StatsDto stats)
    {
        if (!CheckUserTokenId(stats.Token ?? string.Empty, stats.Id)) return NoContent();
        _statsService.AddLoginRecord(new() { Success = true, RegisterTime = DateTime.Now, AutoLogin = true });
        return Ok();
    }
}