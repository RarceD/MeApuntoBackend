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


    // Add to db:
    // _dbContext.Add(test);
    // _dbContext.SaveChanges();

    // Remove from db:
    //_dbContext.Remove(toDel);
    //_dbContext.SaveChanges();

    // Edit db:
    // _dbContext.Update(toEdit);
    // _dbContext.SaveChanges();
}
