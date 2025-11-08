// See https://aka.ms/new-console-template for more information

using System.Drawing;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using projetFinal.Data;
using projetFinal.Models;

#region lancement services


var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Injection de dépendances pour la base de données
var host = Host.CreateDefaultBuilder()
    .ConfigureServices(services =>
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddTransient<DbConnection>();
        services.AddTransient<CSVService>();
    })
    .Build();

// Récupération de base de données créée précédement dans AppDbContext
using var scope = host.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
var dbConnection = scope.ServiceProvider.GetRequiredService<DbConnection>();

#endregion

db.Database.Migrate();

String pathCar = $"/Data/voitures.csv";

string[] lignesCar = File.ReadAllLines(pathCar);

for (int i = 1; i < lignesCar.Length; i++) // On commence à 1 pour sauter l'en-tête
{
    string line = lignesCar[i];
    string[] values = line.Split('/');

    Car car= new Car
    {
        Brand = values[0],
        Model = values[1],
        Year =  int.Parse(values[2]), 
        PriceHT = Convert.ToDecimal(values[3],CultureInfo.InvariantCulture),
        Color = values[4],
        Sold = Convert.ToBoolean(values[5])
    };
    
    db.Cars.Add(car); 
}
db.SaveChanges();


String pathClient = $"{pathProject}/Data/clients.csv"; //pathProject a modifié
var lignesCustomer = File.ReadAllLines(pathClient);

for (int i = 1; i < lignesCustomer.Length; i++) 
{
    string line = lignesCustomer[i];
    string[] values = line.Split('%');

    Client customer = new Client()
    {
        LastName = values[0],
        FirstName = values[1],
        BirthDate= Convert.ToDateTime(values[2]),
        PhoneNumber = values[3],
        Email = values[4]
    };
   db.Clients.Add(customer);
} 

DbConnection dbConnectionService = scope.ServiceProvider.GetRequiredService<DbConnection>();


#region Menu principal

int choice;
do
{
    // Menu
    Console.WriteLine("MENU CONCESSION");
    Console.WriteLine("1) Voir liste voiture");
    Console.WriteLine("2) Historique d'achat (croissant)");
    Console.WriteLine("3) Ajouter un client");
    Console.WriteLine("4) Ajouter une voiture");
    Console.WriteLine("5) Faire un achat de voiture");
    Console.WriteLine("6) Fin");
    Console.Write("Choix : ");

    int.TryParse(Console.ReadLine(), out choice);

    switch (choice)
    {
        // Redirection
        case 1:
            dbConnection.PrintCar();
            break;
        case 2:
            dbConnection.showCarBuy();
            break;
        case 3:
            dbConnection.addClient();
            break;
        case 4:
            dbConnection.userAddCar();
            break;
        case 5:
            dbConnection.FaireAchat();
            break;
        case 6:
            Console.WriteLine("Fin du programme.");
            break;
    }

} while (choice != 6);

#endregion
