﻿using MeApuntoBackend.Repositories;
using MeApuntoBackend.Services.Dto;
using System.Collections.Concurrent;
using System.Timers;

namespace MeApuntoBackend.Services;
public class StatsService : IStatsService
{
    #region Constructor
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ConcurrentStack<LoginRecord> _loginRecords;
    private readonly ConcurrentStack<BookerRecord> _bookerRecords;
    private readonly System.Timers.Timer _dbTimer;
    private const int TIME_STORE_DB_SECONDS = 5 * 1; // Every 1 min I clear db

    public StatsService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        _loginRecords = new();
        _bookerRecords = new();

        _dbTimer = new(TIME_STORE_DB_SECONDS * 1000);
        _dbTimer.Elapsed += (object? source, ElapsedEventArgs e) => StoreInDb();
        _dbTimer.AutoReset = true;
        _dbTimer.Enabled = true;
    }

    #endregion
    public void AddLoginRecord(LoginRecord register) =>
        _loginRecords.Push(register);
    public void AddBookerRecord(BookerRecord register) =>
        _bookerRecords.Push(register);

    #region Private 
    private void StoreInDb()
    {
        using (IServiceScope scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var store = new SchedulerRepository(dbContext);
            while (!_bookerRecords.IsEmpty)
            {
                if (_bookerRecords.TryPop(out var element))
                {
                    // TODO create db table
                }
            }
            while (!_loginRecords.IsEmpty)
            {
                if (_loginRecords.TryPop(out LoginRecord? element))
                {
                    // TODO create db table
                }
            }
        }
    }

    #endregion
}
