// See https://aka.ms/new-console-template for more information
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
            dbConnection.AfficherVoitures();
            break;
        case 2:
            dbConnection.AfficherHistoriqueAchats();
            break;
        case 3:
            dbConnection.AjouterClient();
            break;
        case 4:
            dbConnection.AjouterVoiture();
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
