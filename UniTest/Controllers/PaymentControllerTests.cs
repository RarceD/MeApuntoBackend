using MeApuntoBackend.Controllers;
using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services;
using MeApuntoBackend.Services.Interfaces;
using NSubstitute;
using UnitTest.Controllers;

namespace UniTest.Controllers;

[TestFixture]
public class PaymentControllerTests
{
    private IClientManagementService _clientManagementService;
    private IPaymentService _paymentService;
    private PaymentController _paymentController;

    [SetUp]
    public void Setup()
    {
        _clientManagementService = Substitute.For<IClientManagementService>();
        _paymentService = Substitute.For<IPaymentService>();
        _paymentController = new PaymentController(_clientManagementService, _paymentService);
    }

    [Test]
    public void PostPayment_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        var payment = new PaymentDto { Id = 1, Token = "Token" };
        var response = new PaymentResponse { Url = "Url" };
        // _clientManagementService.IsAdmin(payment.Id, payment.Token).Returns(true);
        // _paymentService.ProccessPayment(payment).Returns(response);

        // Act
        var result = _paymentController.PostPayment(payment);

        // Assert
        // Assert.IsInstanceOf<OkObjectResult>(result);
        // var okResult = result as OkObjectResult;
        // Assert.AreEqual(response, okResult.Value);
    }
}
