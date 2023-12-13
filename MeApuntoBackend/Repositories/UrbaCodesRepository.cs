using MeApuntoBackend.Models;

namespace MeApuntoBackend.Repositories;
public class UrbaCodesRepository : IUrbaCodesRepository
{
    private readonly ApplicationDbContext _context;
    public UrbaCodesRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public void Add(UrbaCodesDb item)
    {
        _context.UrbaCodes.Add(item);
        _context.SaveChanges();
    }
    public void Remove(UrbaCodesDb item)
    {
        _context.UrbaCodes.Remove(item);
        _context.SaveChanges();
    }
    public void Update(UrbaCodesDb item)
    {
        _context.UrbaCodes.Update(item);
        _context.SaveChanges();
    }

    public IEnumerable<UrbaCodesDb> GetAll()
    {
        List<UrbaCodesDb> urbas = _context.UrbaCodes.ToList();
        return urbas;
    }
    public UrbaCodesDb GetById(int id)
    {
        UrbaCodesDb? urba = _context.UrbaCodes.Where(i => i.Id == id).FirstOrDefault();
        if (urba == null) return new UrbaCodesDb();
        return urba;
    }
}
