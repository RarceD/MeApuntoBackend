using MeApuntoBackend.Models;

namespace MeApuntoBackend.Repositories;
public class UrbaRepository : IUrbaRepository
{
    private readonly ApplicationDbContext _context;
    public UrbaRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public void Add(UrbaDb item)
    {
        _context.Urbas.Add(item);
        _context.SaveChanges();
    }
    public void Remove(UrbaDb item)
    {
        _context.Urbas.Remove(item);
        _context.SaveChanges();
    }
    public void Update(UrbaDb item)
    {
        _context.Urbas.Update(item);
        _context.SaveChanges();
    }

    public IEnumerable<UrbaDb> GetAll()
    {
        List<UrbaDb> urbas = _context.Urbas.ToList();
        return urbas;
    }
    public UrbaDb GetById(int id)
    {
        UrbaDb? urba = _context.Urbas.Where(i => i.Id == id).FirstOrDefault();
        if (urba == null) return new UrbaDb();
        return urba;
    }

    public UrbaDb? GetUrbatWithToken(string token)
    {
        UrbaDb? urba = _context.Urbas.Where(i => i.key == token).FirstOrDefault();
        if (urba == null) return new UrbaDb();
        return urba;
    }
}
