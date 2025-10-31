using projetFinal.Models;
namespace projetFinal.Data;


public class DbConnection
{
    private readonly AppDbContext _appDbContext;

    public DbConnection(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public void SaveFullCars(Car cars)
    {
        if (_appDbContext.Cars.Any()) return; // pas de doublon 
        
        _appDbContext.Add(cars);
        _appDbContext.SaveChanges();
    }
}