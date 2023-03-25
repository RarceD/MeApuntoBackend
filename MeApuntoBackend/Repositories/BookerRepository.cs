using MeApuntoBackend.Models;

namespace MeApuntoBackend.Repositories;
public class BookerRepository : IBookerRepository
{
    private readonly ApplicationDbContext _context;
    public BookerRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public IEnumerable<BookerDb> GetAll()
    {
        List<BookerDb> clients = _context.Booker.ToList();
        return clients;
    }
    public BookerDb GetById(int id)
    {
        BookerDb? client = _context.Booker.Where(i => i.Id == id).FirstOrDefault();
        if (client == null) return new BookerDb();
        return client;
    }
}
