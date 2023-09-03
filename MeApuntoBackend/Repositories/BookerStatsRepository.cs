using MeApuntoBackend.Models;

namespace MeApuntoBackend.Repositories;
public class BookerStatsRepository : IBookerStatsRepository
{
    private readonly ApplicationDbContext _context;
    public BookerStatsRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public void Add(BookerStatsDb item)
    {
        _context.BookerStats.Add(item);
        _context.SaveChanges();
    }
    public void Remove(BookerStatsDb item)
    {
        _context.BookerStats.Remove(item);
        _context.SaveChanges();
    }
    public void Update(BookerStatsDb item)
    {
        _context.BookerStats.Update(item);
        _context.SaveChanges();
    }
    public IEnumerable<BookerStatsDb> GetAll()
    {
        return _context.BookerStats.ToList();
    }
    public BookerStatsDb GetById(int id)
    {
        BookerStatsDb? client = _context.BookerStats.Where(i => i.Id == id).FirstOrDefault();
        if (client == null) return new BookerStatsDb();
        return client;
    }
}
