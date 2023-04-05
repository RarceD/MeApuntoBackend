using MeApuntoBackend.Models;

namespace MeApuntoBackend.Repositories;
public class ConfigurationRepository : IConfigurationRepository
{
    private readonly ApplicationDbContext _context;
    public ConfigurationRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public void Add(ConfigurationDb item)
    {
        _context.Configuration.Add(item);
        _context.SaveChanges();
    }
    public void Remove(ConfigurationDb item)
    {
        _context.Configuration.Remove(item);
        _context.SaveChanges();
    }
    public void Update(ConfigurationDb item)
    {
        _context.Configuration.Update(item);
        _context.SaveChanges();
    }
    public IEnumerable<ConfigurationDb> GetAll()
    {
        List<ConfigurationDb> clients = _context.Configuration.ToList();
        return clients;
    }
    public ConfigurationDb GetById(int id)
    {
        ConfigurationDb? client = _context.Configuration.Where(i => i.Id == id).FirstOrDefault();
        if (client == null) return new ConfigurationDb();
        return client;
    }
    public List<ConfigurationDb> GetAllFromCourtId(int courtId)
    {
        List<ConfigurationDb> allAvailableHours = _context.Configuration.Where(i => i.CourtId == courtId).ToList();
        if (allAvailableHours == null) return new List<ConfigurationDb>();
        return allAvailableHours;
    }
}
