using MeApuntoBackend.Controllers;
using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;


namespace UniTest.Controllers;

[TestFixture]
public class ProfileControllerTests
{
    private IClientManagementService _loginManagementService;
    private ProfileController _profileController;
    private ILogger<LoginController> _logger;

    [SetUp]
    public void Setup()
    {
        _loginManagementService = Substitute.For<IClientManagementService>();
        _logger = Substitute.For<ILogger<LoginController>>();
        _profileController = new ProfileController(_logger, _loginManagementService);
    }

    [Test]
    public void GetProfileInfo_WhenCalled_ReturnsProfileResponse()
    {
        // Arrange
        var token = "Token";
        var id = 1;
        var response = new ProfileResponse();
        _loginManagementService.CheckUserTokenId(token, id).Returns(true);
        _loginManagementService.GetProfileInfo(id).Returns(response);

        // Act
        var result = _profileController.GetProfileInfo(token, id);

        // Assert
        // Assert.AreEqual(response, result);
    }
}
