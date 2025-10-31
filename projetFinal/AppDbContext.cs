using projetFinal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace projetFinal;


public class AppDbContext : DbContext
{
    public DbSet<Car> Cars { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Concession> Concessions { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
    {
    }

    public AppDbContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Concession>()
            .HasMany(c => c.Cars)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(@"C:\\Users\\Cam\\Desktop\\SDV\\B2\\projetFinal\\projetFinal\\appsettings.json")
            .Build();

        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        }

        optionsBuilder.UseNpgsql(""); //chemin du postgre
    }
    
}