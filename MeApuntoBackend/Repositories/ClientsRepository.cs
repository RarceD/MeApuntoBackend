using MeApuntoBackend.Models;

namespace MeApuntoBackend.Repositories;
public class ClientsRepository : IClientRepository
{
    private readonly ApplicationDbContext _context;
    public ClientsRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public void Add(ClientDb item)
    {
        _context.Clients.Add(item);
        _context.SaveChanges();
    }
    public void Remove(ClientDb item)
    {
        _context.Clients.Remove(item);
        _context.SaveChanges();
    }
    public void Update(ClientDb item)
    {
        _context.Clients.Update(item);
        _context.SaveChanges();
    }

    public IEnumerable<ClientDb> GetAll()
    {
        List<ClientDb> clients = _context.Clients.ToList();
        return clients;
    }
    public ClientDb GetById(int id)
    {
        ClientDb? client = _context.Clients.Where(i => i.id == id).FirstOrDefault();
        if (client == null) return new ClientDb();
        return client;
    }
    public ClientDb? GetClientWithUser(string username)
    {
        ClientDb? client = _context.Clients.Where(i => i.username == username).FirstOrDefault();
        if (client == null) return null;
        return client;
    }
    public bool IsValidUserCode(string code)
    {
        ClientDb? client = _context.Clients.Where(i => i.name == code).FirstOrDefault();
        if (client == null) return true;
        return false;
    }
    public void UpdateProfileInfo(ClientDb updateClient)
    {
    }
}
