using Microsoft.EntityFrameworkCore;
using SaveSyncro.Models;

namespace SaveSyncro.Contexts;

public class UserContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public UserContext()
    {
        Database.EnsureCreated();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // optionsBuilder.UseNpgsql("Host=192.168.3.34;Port=8082;Database=spotiusersdb;Username=postgres;Password=1234");
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=spotiusersdb;Username=postgres;Password=1234");
    }
}