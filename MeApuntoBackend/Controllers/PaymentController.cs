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

    public class InputDto
    {
        public string Token { get; set; }
        public int Id { get; set; }

    }
    [HttpPost("pay")]
    public IActionResult PostPayment(InputDto input)
    {
        if (!IsAdmin(input.Id, input.Token)) return GetNotAdminResponse();
        var session = _tPVService.ProccessPayment();

        Response.Headers.Add("Location", session.Url);
        return Ok(session);
    }
}