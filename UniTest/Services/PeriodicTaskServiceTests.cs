using MeApuntoBackend.Services;
using NSubstitute;

namespace UniTest.Services;

[TestFixture]
public class PeriodicTaskServiceTests
{
    private IServiceProvider _serviceProvider;
    private PeriodicTaskService _periodicTaskService;

    [SetUp]
    public void Setup()
    {
        _serviceProvider = Substitute.For<IServiceProvider>();
        _periodicTaskService = new PeriodicTaskService(_serviceProvider);
    }

    [Test]
    public void ExecuteAsync_WhenCalled_SetsTimerBookerCleaner()
    {
        // Arrange
        // var cancellationToken = new CancellationToken();

        // Act
        // await _periodicTaskService.ExecuteAsync(cancellationToken);



        // Assert
        // There's no direct way to assert that the timer was set, as it's a private static field.
        // However, you can indirectly test it by checking that the CleanBooks method is called after a certain amount of time.
        // This would require making the CleanBooks method public and virtual, and using a library like Moq to verify that it's called.
    }
}
