using MeApuntoBackend.Models;

namespace MeApuntoBackend.Repositories;
public class CourtRepository : ICourtRepository
{
    private readonly ApplicationDbContext _context;
    public CourtRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public void Add(CourtDb item)
    {
        _context.Courts.Add(item);
        _context.SaveChanges();
    }
    public void Remove(CourtDb item)
    {
        _context.Courts.Remove(item);
        _context.SaveChanges();
    }
    public void Update(CourtDb item)
    {
        _context.Courts.Update(item);
        _context.SaveChanges();
    }
    public IEnumerable<CourtDb> GetAll()
    {
        List<CourtDb> clients = _context.Courts.ToList();
        return clients;
    }
    public CourtDb GetById(int id)
    {
        CourtDb? client = _context.Courts.Where(i => i.Id == id).FirstOrDefault();
        if (client == null) return new CourtDb();
        return client;
    }

    public List<CourtDb> GetFromUrbaId(int urbaId)
    {
        List<CourtDb> clients = _context.Courts.Where(i => i.urba_id == urbaId).ToList();
        return clients;
    }
}
