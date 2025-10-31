using projetFinal.Models;
using System.Linq;

namespace projetFinal.Data;

public class DbConnection
{
    private readonly AppDbContext _dbContext;

    public DbConnection(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Affiche voitures et proprios (info figurants dans les csv)
    public void AfficherVoitures()
    {
        Console.WriteLine("Liste des voitures :");

        foreach (var c in _dbContext.Cars)
        {
            var infoClient = c.Sold && c.Client != null
                ? $" - Propriétaire : {c.Client.FirstName} {c.Client.LastName}"
                : "";

            Console.WriteLine($"{c.Brand} {c.Model} ({c.Year}) - {(c.Sold ? "Vendue" : "À vendre")} - {c.PriceTTC}€ TTC{infoClient}");
        }
    }

    // Affiche historique des achats dans ordre croissant des dates d'achat
    public void AfficherHistoriqueAchats()
    {
        Console.WriteLine("Historique des achats :");

        var achats = _dbContext.Achats
            .OrderBy(a => a.DateAchat)
            .ToList();

        foreach (var a in achats)
        {
            Console.WriteLine($"{a.DateAchat.ToShortDateString()} : {a.Acheteur.FirstName} {a.Acheteur.LastName} a acheté {a.VoitureVendue.Brand} {a.VoitureVendue.Model}");
        }
    }

    // ajoute un client dans la base de données 
    public void AjouterClient()
    {
        Console.Write("Nom : "); string nom = Console.ReadLine();
        Console.Write("Prénom : "); string prenom = Console.ReadLine();
        Console.Write("Email : "); string email = Console.ReadLine();
        Console.Write("Date de naissance (au format année, mois, date) : "); DateTime naissance = DateTime.Parse(Console.ReadLine());
        Console.Write("Téléphone : "); string tel = Console.ReadLine();

        var client = new Client
        {
            LastName = nom,
            FirstName = prenom,
            Email = email,
            BirthDate = naissance,
            PhoneNumber = tel
        };

        _dbContext.Clients.Add(client);
        _dbContext.SaveChanges();

        Console.WriteLine("Client ajouté avec succès.");
    }

    // ajoute une voiture dans la base de données 
    public void AjouterVoiture()
    {
        Console.Write("Marque : "); string marque = Console.ReadLine();
        Console.Write("Modèle : "); string modele = Console.ReadLine();
        Console.Write("Année : "); int annee = int.Parse(Console.ReadLine());
        Console.Write("Prix HT : "); decimal prix = decimal.Parse(Console.ReadLine());
        Console.Write("Couleur : "); string couleur = Console.ReadLine();

        var voiture = new Car
        {
            Brand = marque,
            Model = modele,
            Year = annee,
            PriceHT = prix,
            Color = couleur,
            Sold = false
        };

        _dbContext.Cars.Add(voiture);
        _dbContext.SaveChanges();

        Console.WriteLine("Voiture ajoutée avec succès.");
    }

    // lie client et voiture crée un ACHAT et sauvegarde
    public void FaireAchat()
    {
        Console.Write("ID du client : "); Guid idClient = Guid.Parse(Console.ReadLine());
        Console.Write("ID de la voiture : "); Guid idVoiture = Guid.Parse(Console.ReadLine());

        var client = _dbContext.Clients.Find(idClient);
        var voiture = _dbContext.Cars.Find(idVoiture);

        if (client != null && voiture != null && !voiture.Sold)
        {
            voiture.Sold = true;
            voiture.Client = client;

            var achat = new Achat
            {
                Acheteur = client,
                VoitureVendue = voiture,
                DateAchat = DateTime.Now
            };

            _dbContext.Achats.Add(achat);
            _dbContext.SaveChanges();

            Console.WriteLine("Achat effectué avec succès.");
        }
        else
        {
            Console.WriteLine("Erreur : client introuvable, voiture introuvable ou déjà vendue.");
        }
    }
}
