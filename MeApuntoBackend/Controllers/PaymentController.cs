using MeApuntoBackend.Services;
using MeApuntoBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MeApuntoBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : GenericController
{
    private readonly IPaymentService _tPVService;
    public PaymentController(
        IClientManagementService clientManagementService,
        IPaymentService tpv
        ) : base(clientManagementService)
    {
        _tPVService = tpv;
    }

    [HttpPost("pay")]
    public IActionResult GetMatchCode(string token, int id, string matchStr)
    {
        if (!IsAdmin(id, token)) return GetNotAdminResponse();
        _tPVService.ProccessPayment();
        return Ok(_clientManagementService.GetCodeContains(matchStr.ToLower()));
    }
}