using MeApuntoBackend.Controllers;
using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services;
using NSubstitute;

namespace UniTest.Controllers;

[TestFixture]
public class NormativeControllerTests
{
    private IClientManagementService _loginManagementService;
    private ICourtManagementService _courtManagementService;
    private NormativeController _normativeController;

    [SetUp]
    public void Setup()
    {
        _loginManagementService = Substitute.For<IClientManagementService>();
        _courtManagementService = Substitute.For<ICourtManagementService>();
        _normativeController = new NormativeController(_loginManagementService, _courtManagementService);
    }

    [Test]
    public void GetNormative_WhenCalled_ReturnsListOfNormativeResponse()
    {
        // Arrange
        var token = "Token";
        var id = 1;
        var response = new List<NormativeResponse>();
        _loginManagementService.CheckUserTokenId(token, id).Returns(true);
        _courtManagementService.GetNormativeByClientId(id).Returns(response);

        // Act
        var result = _normativeController.GetNormative(token, id);

        // Assert
        // Assert.AreEqual(response, result);
    }
}
