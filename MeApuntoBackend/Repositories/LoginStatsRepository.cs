using MeApuntoBackend.Models;

namespace MeApuntoBackend.Repositories;
public class LoginStatsRepository : ILoginStatsRepository
{
    private readonly ApplicationDbContext _context;
    public LoginStatsRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public void Add(LoginStatsDb item)
    {
        _context.LoginStats.Add(item);
        _context.SaveChanges();
    }
    public void Remove(LoginStatsDb item)
    {
        _context.LoginStats.Remove(item);
        _context.SaveChanges();
    }
    public void Update(LoginStatsDb item)
    {
        _context.LoginStats.Update(item);
        _context.SaveChanges();
    }
    public IEnumerable<LoginStatsDb> GetAll()
    {
        List<LoginStatsDb> clients = _context.LoginStats.ToList();
        return clients;
    }
    public LoginStatsDb GetById(int id)
    {
        LoginStatsDb? client = _context.LoginStats.Where(i => i.Id == id).FirstOrDefault();
        if (client == null) return new LoginStatsDb();
        return client;
    }
}
