
using MeApuntoBackend.Controllers;
using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UniTest.Controllers;

[TestFixture]
public class CreateControllerTests
{
    private IClientManagementService _loginManagementService;
    private CreateController _createController;
    private ILogger<LoginController> _logger;

    [SetUp]
    public void Setup()
    {
        _loginManagementService = Substitute.For<IClientManagementService>();
        _logger = Substitute.For<ILogger<LoginController>>();
        _createController = new CreateController(_logger, _loginManagementService);
    }

    [Test]
    public void CreateUser_WhenCalled_ReturnsActionResult()
    {
        // Arrange
        var input = new CreateDto { User = "User", Pass = "Pass", Name = "Name", Key = "Key" };
        _loginManagementService.AddClient(input, 1).Returns(true);

        // Act
        var result = _createController.CreateUser(input);

        // Assert
        // Assert.IsInstanceOf<OkResult>(result);
    }
}
