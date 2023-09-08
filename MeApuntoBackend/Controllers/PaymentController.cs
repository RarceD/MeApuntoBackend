using MeApuntoBackend.Controllers.Dtos;
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
    public IActionResult PostPayment(PaymentDto payment)
    {
        if (!IsAdmin(payment.Id, payment.Token ?? string.Empty)) return GetNotAdminResponse();
        var response = _tPVService.ProccessPayment(payment);
        Response.Headers.Add("Location", response.Url);
        return Ok(response);
    }
}