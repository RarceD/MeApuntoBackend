using MeApuntoBackend.Models;

namespace MeApuntoBackend.Repositories;
public class NormativeRepository : INormativeRepository
{
    private readonly ApplicationDbContext _context;
    public NormativeRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public void Add(NormativeDb item)
    {
        _context.Normative.Add(item);
        _context.SaveChanges();
    }
    public void Remove(NormativeDb item)
    {
        _context.Normative.Remove(item);
        _context.SaveChanges();
    }
    public void Update(NormativeDb item)
    {
        _context.Normative.Update(item);
        _context.SaveChanges();
    }
    public IEnumerable<NormativeDb> GetAll()
    {
        List<NormativeDb> normatives = _context.Normative.ToList();
        return normatives;
    }
    public NormativeDb GetById(int id)
    {
        throw new NotImplementedException();
    }
    public IEnumerable<NormativeDb> GetAllFromUrbaId(int id)
    {
        List<NormativeDb> normatives = _context.Normative.Where(i => i.UrbaId == id).ToList();
        return normatives;
    }
}
