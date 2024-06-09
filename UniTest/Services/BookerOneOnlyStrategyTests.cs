using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Models;
using MeApuntoBackend.Repositories;
using MeApuntoBackend.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UniTest.Services;

[TestFixture]
public class BookerOneOnlyStrategyTests
{
    private ISchedulerRepository _schedulerRepository;
    private IConfigurationRepository _configurationRepository;
    private IMailService _mailService;
    private ILogger<BookerService> _logger;
    private BookerOneOnlyStrategy _bookerOneOnlyStrategy;

    [SetUp]
    public void Setup()
    {
        _schedulerRepository = Substitute.For<ISchedulerRepository>();
        _configurationRepository = Substitute.For<IConfigurationRepository>();
        _mailService = Substitute.For<IMailService>();
        _logger = Substitute.For<ILogger<BookerService>>();
        _bookerOneOnlyStrategy = new BookerOneOnlyStrategy(_schedulerRepository, _configurationRepository, _mailService, _logger);
    }

    [Test]
    public void ValidDayHour_WhenCalled_ReturnsExpectedResult()
    {
        // Arrange
        var newBook = new BookerDto { CourtId = 1, Time = "10:00", Day = "2022-12-31", Duration = DurationType.ONE_HOUR, Id = 1 };
        // var clientId = 1;
        _configurationRepository.GetAllFromCourtId(newBook.CourtId).Returns(new List<ConfigurationDb> { new ConfigurationDb { ValidHour = "10:00" } });
        //_schedulerRepository.GetByClientId(clientId).Returns(new List<BookerDto>());
        //_schedulerRepository.GetBookInDay(newBook.Day).Returns(new List<BookerDto>());

        // Act
        // var result = _bookerOneOnlyStrategy.ValidDayHour(newBook, clientId);

        // Assert
        // Assert.That(result, Is.True, "It is not valid Hour");
    }
}
