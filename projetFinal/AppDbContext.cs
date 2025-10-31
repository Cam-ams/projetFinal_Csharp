using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using projetFinal.Models;

namespace projetFinal.Data;

public class AppDbContext : DbContext
{
    // Créations des tables pour la base de données sur pgAdmin
    public DbSet<Client> Clients { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<Achat> Achats { get; set; }
    public DbSet<Concession> Concessions { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public AppDbContext() { }

    // Connexion via appsettings.json
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        }
    }
}