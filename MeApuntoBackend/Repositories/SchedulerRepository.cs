﻿using MeApuntoBackend.Models;

namespace MeApuntoBackend.Repositories;
public class SchedulerRepository : ISchedulerRepository
{
    private readonly ApplicationDbContext _context;
    public SchedulerRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public void Add(SchedulerDb item)
    {
        _context.Scheduler.Add(item);
        _context.SaveChanges();
    }
    public void Remove(SchedulerDb item)
    {
        _context.Scheduler.Remove(item);
        _context.SaveChanges();
    }
    public void Update(SchedulerDb item)
    {
        _context.Scheduler.Update(item);
        _context.SaveChanges();
    }
    public IEnumerable<SchedulerDb> GetAll()
    {
        List<SchedulerDb> clients = _context.Scheduler.ToList();
        return clients;
    }
    public SchedulerDb GetById(int id)
    {
        SchedulerDb? client = _context.Scheduler.Where(i => i.Id == id).FirstOrDefault();
        if (client == null) return new SchedulerDb();
        return client;
    }

    public List<SchedulerDb> GetBookInDay(string day)
    {
        List<SchedulerDb> ocupied = _context.Scheduler.Where(i => i.Day == day).ToList();
        return ocupied;
    }
    public List<SchedulerDb> GetBooksByCourtId(int id)
    {
        List<SchedulerDb> ocupied = _context.Scheduler.Where(i => i.CourtId == id).ToList();
        return ocupied;
    }
    public List<SchedulerDb> GetByClientId(int clientId)
        => _context.Scheduler.Where(i => i.ClientId == clientId).ToList();

    public string ToPrint(SchedulerDb s) =>
        $"CourtId:{s.CourtId} - ClientId: {s.ClientId} - Time:{s.Time} - {s.Day}";
}
