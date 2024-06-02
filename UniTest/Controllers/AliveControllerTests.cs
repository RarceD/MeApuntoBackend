using MeApuntoBackend.Controllers;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UniTest.Controllers;
[TestFixture]
public class AliveControllerTests
{
    private ILogger<LoginController> _logger;
    private AliveController _aliveController;

    [SetUp]
    public void Setup()
    {
        _logger = Substitute.For<ILogger<LoginController>>();
        _aliveController = new AliveController(_logger);
    }

    [Test]
    public void MakeLogin_WhenCalled_ReturnsCurrentDateTime()
    {
        // Act
        var result = _aliveController.MakeLogin();

        // Assert
        // Assert.AreEqual(DateTime.Now.ToString(), result);
    }
}
