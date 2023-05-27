using MeApuntoBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace MeApuntoBackend.Repositories;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }
    public DbSet<ClientDb> Clients { get; set; }
    public DbSet<CourtDb> Courts { get; set; }
    public DbSet<NormativeDb> Normative { get; set; }
    public DbSet<UrbaDb> Urbas { get; set; }
    public DbSet<SchedulerDb> Scheduler { get; set; }
    public DbSet<ConfigurationDb> Configuration { get; set; }
}
