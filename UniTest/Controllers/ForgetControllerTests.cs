using MeApuntoBackend.Controllers;
using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UniTest.Controllers;
[TestFixture]
public class ForgetControllerTests
{
    private IClientManagementService _loginManagementService;
    private ForgetController _forgetController;
    private ILogger<LoginController> _logger;

    [SetUp]
    public void Setup()
    {
        _loginManagementService = Substitute.For<IClientManagementService>();
        _logger = Substitute.For<ILogger<LoginController>>();
        _forgetController = new ForgetController(_logger, _loginManagementService);
    }

    [Test]
    public void ForgetFuckingPass_WhenCalled_ReturnsForgetResponse()
    {
        // Arrange
        var input = new ForgetDto { Username = "Username" };
        _loginManagementService.ForgetPassword(input.Username).Returns(true);

        // Act
        var result = _forgetController.ForgetFuckingPass(input);

        // Assert
        // Assert.IsTrue(result.Success);
    }
}
