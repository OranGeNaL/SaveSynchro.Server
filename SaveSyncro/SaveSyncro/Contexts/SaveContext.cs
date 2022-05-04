using Microsoft.EntityFrameworkCore;
using SaveSyncro.Models;

namespace SaveSyncro.Contexts;

public class SaveContext : DbContext
{
    public DbSet<Save> Saves { get; set; }

    public SaveContext()
    {
        Database.EnsureCreated();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Saves;Username=postgres;Password=1234");
    }
}