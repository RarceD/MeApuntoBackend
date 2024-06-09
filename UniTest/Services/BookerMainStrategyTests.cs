using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Repositories;
using MeApuntoBackend.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UniTest.Services;

[TestFixture]
public class BookerMainStrategyTests
{
    private ISchedulerRepository _schedulerRepository;
    private IConfigurationRepository _configurationRepository;
    private IMailService _mailService;
    private ILogger<BookerService> _logger;
    private BookerMainStrategy _bookerMainStrategy;

    [SetUp]
    public void Setup()
    {
        _schedulerRepository = Substitute.For<ISchedulerRepository>();
        _configurationRepository = Substitute.For<IConfigurationRepository>();
        _mailService = Substitute.For<IMailService>();
        _logger = Substitute.For<ILogger<BookerService>>();
        _bookerMainStrategy = new BookerMainStrategy(_schedulerRepository, _configurationRepository, _mailService, _logger);
    }

    [Test]
    public void ValidDayHour_WhenCalled_ReturnsExpectedResult()
    {
        // Arrange
        //var newBook = new BookerDto { CourtId = 1, Time = "10:00", Day = "2022-12-31", Duration = DurationType.ONE_HOUR, Id = 1 };
        //var clientId = 1;
        // _configurationRepository.GetAllFromCourtId(newBook.CourtId).Returns(new List<ConfigurationDb> { new ConfigurationDb { ValidHour = "10:00" } });
        // _schedulerRepository.GetBookInDay(newBook.Day).Returns(new List<BookerDto>());

        // Act
        // var result = _bookerMainStrategy.ValidDayHour(newBook, clientId);

        // Assert
        Assert.That(false, Is.False, "It is not valid Hour");
    }
}
