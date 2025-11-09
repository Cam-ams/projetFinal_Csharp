using System.Globalization;
using projetFinal.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace projetFinal.Data;

public class DbConnection
{
    private readonly AppDbContext _dbContext;

    public DbConnection(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Affiche voitures et proprios (info figurants dans les csv)
    public void PrintCar()
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
    public void showCarBuy()
    {
        Console.WriteLine("Historique des achats :");
        var achats = _dbContext.Achats
            .Include(a => a.Acheteur)
            .Include(a => a.VoitureVendue)
            .OrderBy(a => a.DateAchat)
            .ToList();
 
        foreach (var a in achats)
        {
            Console.WriteLine($"{a.DateAchat.ToShortDateString()} : {a.Acheteur.FirstName} {a.Acheteur.LastName} a acheté {a.VoitureVendue.Brand} {a.VoitureVendue.Model}");
        }
    }

    // ajoute un client dans la base de données 
    public void userAddClient()
    {
        Console.Write("Nom : "); string nom = Console.ReadLine();
        Console.Write("Prénom : "); string prenom = Console.ReadLine();
        Console.Write("Email : "); string email = Console.ReadLine();
        Console.Write("Date de naissance (au format année, mois, date) : ");
        DateTime naissance = DateTime.SpecifyKind(DateTime.Parse(Console.ReadLine()), DateTimeKind.Utc);
        Console.Write("Téléphone : "); string tel = Console.ReadLine();

        Client client = new Client
        {
            LastName = nom,
            FirstName = prenom,
            Email = email,
            BirthDate = naissance,
            PhoneNumber = tel
        };
        addClient(client);
    }
    public void addClient(Client client)
    {
        bool isClientExist = _dbContext.Clients.Any(c => c.LastName == client.LastName && c.FirstName == client.FirstName && c.BirthDate == client.BirthDate );

        if (!isClientExist){
            _dbContext.Clients.Add(client);
            _dbContext.SaveChanges();
            Console.WriteLine("Client ajoutée avec succès.");

        }else 
        {
            Console.WriteLine("Client existe deja.");
        }
    }

    
    // ajoute une voiture dans la base de données 
    public void userAddCar()
    {
        Console.Write("Marque : "); string marque = Console.ReadLine();
        Console.Write("Modèle : "); string modele = Console.ReadLine();
        Console.Write("Année : "); int annee = int.Parse(Console.ReadLine());
        Console.Write("Prix HT : "); decimal prix = decimal.Parse(Console.ReadLine());
        Console.Write("Couleur : "); string couleur = Console.ReadLine();

        Car voiture = new Car
        {
            Brand = marque,
            Model = modele,
            Year = annee,
            PriceHT = prix,
            Color = couleur,
            Sold = false
        };
        addCar(voiture);
    }

    public void addCar( Car voiture)
    {
         
        bool isCarExist = _dbContext.Cars.Any(c => c.Model == voiture.Model && c.PriceHT == voiture.PriceHT && c.Brand == voiture.Brand && c.Year == voiture.Year && c.Color == voiture.Color);

        if (!isCarExist){
            _dbContext.Cars.Add(voiture);
            _dbContext.SaveChanges();
            Console.WriteLine("Voiture ajoutée avec succès.");

        }else 
        {
            Console.WriteLine("Voiture existe deja.");
        }
    }

    // lie client et voiture crée un ACHAT et sauvegarde
    public void FaireAchat()
    {
        Console.Write("Nom du client : "); string NameClient = Console.ReadLine();
        List<Client> matchedClients = _dbContext.Clients.Where(c => c.LastName.ToLower() == NameClient.ToLower()).ToList();
       
        Client client; 
        
        if (matchedClients.Count == 0)
        {
            Console.WriteLine("Le client n'est pas trouvé");
            FaireAchat();
            return;
        }
        else if(matchedClients.Count > 1)
        {
            Console.WriteLine("Choisir parmis les clients trouvés");
            for (int i = 0; i < matchedClients.Count ; i++)
            {
                Console.WriteLine($"{i}) {matchedClients[i].LastName} {matchedClients[i].FirstName} {matchedClients[i].BirthDate} ");

            }
            
            int indexClient;
            Console.Write("Choix : ");
            int.TryParse(Console.ReadLine(), out indexClient);
            client =  matchedClients[indexClient];
        }
        else
        {
            client =  matchedClients[0];
        }
        
        Console.Write("Modèle de la voiture : ");string modelCar = Console.ReadLine();
        List<Car> matchedCars = _dbContext.Cars.Where(c => c.Model.ToLower() == modelCar.ToLower() && c.Sold == false).ToList();
       
        Car car; 
        
        if (matchedCars.Count == 0)
        {
            Console.WriteLine("Ce modèle n'est pas disponible !");
            return;
        }
        else if(matchedCars.Count > 1)
        {
            Console.WriteLine("Choisir parmis les voitures trouvés");
            for (int i = 0; i < matchedCars.Count ; i++)
            {
                Console.WriteLine($"{i}) {matchedCars[i].Brand} {matchedCars[i].Model}, {matchedCars[i].PriceTTC}€, {matchedCars[i].Year}");
            }
            
            int indexCar;
            Console.Write("Choix : ");
            int.TryParse(Console.ReadLine(), out indexCar);
            car = matchedCars[indexCar];
        }
        else
        { 
            car = matchedCars[0];
        }
        
        car.Sold = true; 
        car.Client = client;
        Achat achat = new Achat
        {
            Acheteur = client,
            VoitureVendue = car,
            DateAchat = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)
        };
        
        _dbContext.Achats.Add(achat);
        _dbContext.SaveChanges();
        
        Console.WriteLine("Achat effectué avec succès.");
    }

    public void loadCsv()
    {
        string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"../../.."));

        String pathCar = $"{projectRoot}/Data/voitures.csv";

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

            addCar(car);
        }


        String pathClient = $"{projectRoot}/Data/clients.csv"; 
        var lignesCustomer = File.ReadAllLines(pathClient);

        for (int i = 1; i < lignesCustomer.Length; i++) 
        {
            string line = lignesCustomer[i];
            string[] values = line.Split('%');

            Client customer = new Client()
            {
                LastName = values[0],
                FirstName = values[1],
                BirthDate= DateTime.SpecifyKind(DateTime.Parse(values[2]), DateTimeKind.Utc),
                PhoneNumber = values[3],
                Email = values[4]
            };
            addClient(customer);
        } 

    }

    public void resetBdd()
    {
        _dbContext.Clients.RemoveRange(_dbContext.Clients);
        _dbContext.Cars.RemoveRange(_dbContext.Cars);
        _dbContext.SaveChanges();
       loadCsv();
        Console.WriteLine("La base donnée est réinitialisée");
    }
    
    


}
