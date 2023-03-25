using MeApuntoBackend.Models;

namespace MeApuntoBackend.Repositories;
public class ClientsRepository : IClientRepository
{
    private readonly ApplicationDbContext _context;
    public ClientsRepository(ApplicationDbContext context)
    {
        _context = context;
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
}
