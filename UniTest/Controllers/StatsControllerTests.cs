using MeApuntoBackend.Controllers;
using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;


namespace UniTest.Controllers;

[TestFixture]
public class StatsControllerTests
{
    private IClientManagementService _loginManagementService;
    private IBookerManagementService _bookerManagementService;
    private IStatsService _statsService;
    private StatsController _statsController;
    private ILogger<StatsController>? _logger;

    [SetUp]
    public void Setup()
    {
        _loginManagementService = Substitute.For<IClientManagementService>();
        _bookerManagementService = Substitute.For<IBookerManagementService>();
        _statsService = Substitute.For<IStatsService>();
        _logger = Substitute.For<ILogger<StatsController>>();
        _statsController = new StatsController(_logger, _loginManagementService, _bookerManagementService, _statsService);
    }

    [Test]
    public void UpdateProfileInfo_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        var input = new StatsDto { Token = "Token", Id = 1 };
        _loginManagementService.CheckUserTokenId(input.Token, input.Id).Returns(true);

        // Act
        var result = _statsController.UpdateProfileInfo(input);

        // Assert
        // Assert.IsInstanceOf<OkResult>(result);
    }

    // Add more tests for the other methods in the StatsController
}
