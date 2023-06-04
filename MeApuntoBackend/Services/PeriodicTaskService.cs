using MeApuntoBackend.Repositories;
using System.Timers;

namespace MeApuntoBackend.Services;
public class PeriodicTaskService : BackgroundService
{
    private static ISchedulerRepository? _schedulerRepository;
    private static System.Timers.Timer? _bookerTimer;
    private static IServiceProvider? _serviceProvider;
    private static ILogger<PeriodicTaskService>? _logger;
    private const int TIME_CLEAN_DB_SECONDS = 60 * 30; // Every 30 min I clear db
    public PeriodicTaskService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private static void SetTimerBookerCleaner()
    {
        _bookerTimer = new System.Timers.Timer(TIME_CLEAN_DB_SECONDS * 1000);
        _bookerTimer.Elapsed += (object? source, ElapsedEventArgs e) => CleanBooks();
        _bookerTimer.AutoReset = true;
        _bookerTimer.Enabled = true;
    }
    private static void CleanBooks()
    {
        if (_serviceProvider == null) return;
        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _logger = scope.ServiceProvider.GetRequiredService<ILogger<PeriodicTaskService>>();
            _logger.LogWarning("[PERIODIC DELETE] at:" + DateTime.Now.ToString());
            _schedulerRepository = new SchedulerRepository(dbContext);
            ClearScheduleTable();
        }
    }
    private static void ClearScheduleTable()
    {
        if (_schedulerRepository == null) return;
        var allSchedules = _schedulerRepository.GetAll();

        var yestardayDay = DateTime.Now.AddDays(-1).ToShortDateString();

        foreach (var scheduler in allSchedules)
        {
            if (scheduler.Day == yestardayDay)
            {
                _schedulerRepository.Remove(scheduler);
                _logger?.LogWarning("[PERIODIC DELETE] task: " + _schedulerRepository.ToPrint(scheduler));

            }
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Set timer to execute periodically:
        _ = Task.Run(() =>
        {
            SetTimerBookerCleaner();
        });
        return Task.CompletedTask;
    }
}
