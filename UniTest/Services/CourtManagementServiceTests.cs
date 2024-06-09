using MeApuntoBackend.Models;
using MeApuntoBackend.Repositories;
using MeApuntoBackend.Services;
using NSubstitute;

namespace UniTest.Services;

[TestFixture]
public class CourtManagementServiceTests
{
    private IClientRepository _clientRepository;
    private IUrbaRepository _urbaRepository;
    private INormativeRepository _normativeRepository;
    private ISchedulerRepository _schedulerRepository;
    private ICourtRepository _courtRepository;
    private IConfigurationRepository _configurationRepository;
    private CourtManagementService _courtManagementService;

    [SetUp]
    public void Setup()
    {
        _clientRepository = Substitute.For<IClientRepository>();
        _urbaRepository = Substitute.For<IUrbaRepository>();
        _normativeRepository = Substitute.For<INormativeRepository>();
        _schedulerRepository = Substitute.For<ISchedulerRepository>();
        _courtRepository = Substitute.For<ICourtRepository>();
        _configurationRepository = Substitute.For<IConfigurationRepository>();
        _courtManagementService = new CourtManagementService(_clientRepository, _urbaRepository, _schedulerRepository, _normativeRepository, _configurationRepository, _courtRepository);
    }

    [Test]
    public void GetCourts_WhenCalled_ReturnsExpectedResult()
    {
        // Arrange
        var clientId = 1;
        var client = new ClientDb { id = clientId, urba_id = 1 };
        var court = new CourtDb { Id = 1, name = "Court 1", type = 0, valid_times = "10:00-11:00" };
        var urba = new UrbaDb { Id = 1, advance_book = 1 };
        _clientRepository.GetById(clientId).Returns(client);
        _courtRepository.GetFromUrbaId(client.urba_id).Returns(new List<CourtDb> { court });
        _urbaRepository.GetById(client.urba_id).Returns(urba);

        // Act
        var result = _courtManagementService.GetCourts(clientId);

        // Assert
        //Assert.That(1, Is.EqualTo(result.Count()));
        //Assert.That(court.Id, Is.EqualTo(result.First().Id));
        //Assert.That(court.name, Is.EqualTo(result.First().Name));
        //Assert.That(court.type, Is.EqualTo(result.First().Type));
        //Assert.That(court.valid_times, Is.EqualTo(result.First().ValidTimes));
    }

    [Test]
    public void GetNormativeByClientId_WhenCalled_ReturnsExpectedResult()
    {
        // Arrange
        var clientId = 1;
        var client = new ClientDb { id = clientId, urba_id = 1 };
        var normative = new NormativeDb { Id = 1, Text = "Text 1", Title = "Title 1" };
        _clientRepository.GetById(clientId).Returns(client);
        _normativeRepository.GetAllFromUrbaId(client.urba_id).Returns(new List<NormativeDb> { normative });

        // Act
        var result = _courtManagementService.GetNormativeByClientId(clientId);

        // Assert
        Assert.That(1, Is.EqualTo(result.Count()));
        Assert.That(normative.Id, Is.EqualTo(result.First().Id));
        Assert.That(normative.Text, Is.EqualTo(result.First().Text));
        Assert.That(normative.Title, Is.EqualTo(result.First().Title));
    }
}
