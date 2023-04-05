using MeApuntoBackend.Repositories;
using System.Timers;

namespace MeApuntoBackend.Services;
public class PeriodicTaskService : BackgroundService
{
    private static IBookerRepository? _bookerRepository;
    private static System.Timers.Timer? _bookerTimer;
    private static IServiceProvider? _serviceProvider;
    public PeriodicTaskService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private static void SetTimerBookerCleaner()
    {
        _bookerTimer = new System.Timers.Timer(4000);
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
            var b = _bookerRepository.GetById(907);
            Console.WriteLine(b.court_id);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Set timer to execute periodically:
        SetTimerBookerCleaner();

        // Run forever:
        while (!stoppingToken.IsCancellationRequested) await Task.Run(() => { });
        //await Task.Delay(TimeSpan.FromSeconds(1000), stoppingToken);
    }
}
