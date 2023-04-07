using MeApuntoBackend.Repositories;
using System.Timers;

namespace MeApuntoBackend.Services;
public class PeriodicTaskService : BackgroundService
{
    private static IBookerRepository? _bookerRepository;
    private static ISchedulerRepository? _schedulerRepository;
    private static System.Timers.Timer? _bookerTimer;
    private static IServiceProvider? _serviceProvider;
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
        Console.WriteLine("Cleaning books");
        if (_serviceProvider == null) return;
        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _bookerRepository = new BookerRepository(dbContext);
            _schedulerRepository = new SchedulerRepository(dbContext);
            ClearBookerTable();
            ClearScheduleTable();
        }
    }

    private static void ClearBookerTable()
    {
        if ( _bookerRepository == null) return;
        var allBooks = _bookerRepository.GetAll();

        var yesterdayWeekday = (int)DateTime.Now.DayOfWeek - 1;
        if (yesterdayWeekday < 0) yesterdayWeekday = 6;

        var yestardayDay = DateTime.Now.AddDays(-1).ToShortDateString();

        foreach (var booker in allBooks)
        {
            if (booker.weekday == yesterdayWeekday && booker.Day == yestardayDay)
                _bookerRepository.Remove(booker);
        }
    }

    private static void ClearScheduleTable()
    {
        if ( _schedulerRepository == null) return;
        var allSchedules = _schedulerRepository.GetAll();

        var yestardayDay = DateTime.Now.AddDays(-1).ToShortDateString();

        foreach (var scheduler in allSchedules)
        {
            if (scheduler.Day == yestardayDay)
                _schedulerRepository.Remove(scheduler);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Set timer to execute periodically:
        SetTimerBookerCleaner();

        // Run forever:
        while (!stoppingToken.IsCancellationRequested) await Task.Run(() => { });
    }
}
