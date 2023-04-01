using MeApuntoBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace MeApuntoBackend.Repositories;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }
    public DbSet<BookerDb> Booker { get; set; }
    public DbSet<ClientDb> Clients { get; set; }
    public DbSet<CourtDb> Courts { get; set; }
    public DbSet<NormativeDb> Normative { get; set; }
    public DbSet<UrbaDb> Urbas { get; set; }
}
