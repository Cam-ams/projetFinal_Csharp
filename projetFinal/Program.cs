// See https://aka.ms/new-console-template for more information
using projetFinal.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using projetFinal;

class Program
{
    static void Main()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddDbContext<AppDbContext>();
            })
            .Build();

        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        int choice;
        do
        {
            Console.WriteLine("MENU CONCESSION");
            Console.WriteLine("1) Voir liste voiture");
            Console.WriteLine("2) Historique d'achat (croissant)");
            Console.WriteLine("3) Ajouter un Client");
            Console.WriteLine("4) Ajouter une Voiture");
            Console.WriteLine("5) Faire un achat de Voiture");
            Console.WriteLine("6) Fin");
            Console.Write("Choix : ");

            int.TryParse(Console.ReadLine(), out choice);

            switch (choice)
            {
                case 1: VoirListeVoiture(db); break;
                case 2: HistoriqueAchats(db); break;
                case 3: AjouterClient(db); break;
                case 4: AjouterVoiture(db); break;
                case 5: FaireAchat(db); break;
                case 6: Console.WriteLine("Fin du programme."); break;
            }

        } while (choice != 6);
    }

    static void VoirListeVoiture(AppDbContext db)
    {
        Console.WriteLine("Liste des voitures :");
        foreach (var c in db.Cars)
        {
            string infoClient = c.Sold && c.Client != null
                ? $" - Propriétaire : {c.Client.FirstName} {c.Client.LastName}"
                : "";
            Console.WriteLine($"{c.Brand} {c.Model} ({c.Year}) - {(c.Sold ? "Vendue" : "À vendre")} - {c.PriceTTC}€ TTC{infoClient}");
        }
    }

    static void HistoriqueAchats(AppDbContext db)
    {
        Console.WriteLine("Historique des achats :");
        foreach (var a in db.Achats.OrderBy(a => a.DateAchat))
        {
            Console.WriteLine($"{a.DateAchat.ToShortDateString()} : {a.Acheteur.FirstName} {a.Acheteur.LastName} a acheté {a.VoitureVendue.Brand} {a.VoitureVendue.Model}");
        }
    }

    static void AjouterClient(AppDbContext db)
    {
        Console.Write("Nom : "); string lastName = Console.ReadLine();
        Console.Write("Prénom : "); string firstName = Console.ReadLine();
        Console.Write("Email : "); string email = Console.ReadLine();
        Console.Write("Date de naissance (yyyy-MM-dd) : "); DateTime birthDate = DateTime.Parse(Console.ReadLine());
        Console.Write("Téléphone : "); string phone = Console.ReadLine();

        var client = new Client
        {
            LastName = lastName,
            FirstName = firstName,
            Email = email,
            BirthDate = birthDate,
            PhoneNumber = phone
        };

        db.Clients.Add(client);
        db.SaveChanges();
        Console.WriteLine("Client ajouté !");
    }

    static void AjouterVoiture(AppDbContext db)
    {
        Console.Write("Marque : "); string brand = Console.ReadLine();
        Console.Write("Modèle : "); string model = Console.ReadLine();
        Console.Write("Année : "); int year = int.Parse(Console.ReadLine());
        Console.Write("Prix HT : "); decimal priceHT = decimal.Parse(Console.ReadLine());
        Console.Write("Couleur : "); string color = Console.ReadLine();

        var car = new Car
        {
            Brand = brand,
            Model = model,
            Year = year,
            PriceHT = priceHT,
            Color = color,
            Sold = false
        };

        db.Cars.Add(car);
        db.SaveChanges();
        Console.WriteLine("Voiture ajoutée !");
    }

    static void FaireAchat(AppDbContext db)
    {
        Console.Write("ID du client : "); Guid clientId = Guid.Parse(Console.ReadLine());
        Console.Write("ID de la voiture : "); Guid carId = Guid.Parse(Console.ReadLine());

        var client = db.Clients.Find(clientId);
        var car = db.Cars.Find(carId);

        if (client != null && car != null && !car.Sold)
        {
            car.Sold = true;
            car.Client = client;

            var achat = new Achat
            {
                Acheteur = client,
                VoitureVendue = car,
                DateAchat = DateTime.Now
            };

            db.Achats.Add(achat);
            db.SaveChanges();
            Console.WriteLine("Achat effectué !");
        }
    }
}
