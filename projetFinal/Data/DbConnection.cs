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

        foreach (Car c in _dbContext.Cars)
        {
            string infoClient = c.Sold && c.Client != null
                ? $" - Propriétaire : {c.Client.FirstName} {c.Client.LastName}"
                : "";

            Console.WriteLine($"{c.Brand} {c.Model} ({c.Year}) - {(c.Sold ? "Vendue" : "À vendre")} - {c.PriceTTC}€ TTC{infoClient}");
        }
    }

    // Affiche historique des achats dans ordre croissant des dates d'achat
    public void displayCarSales()
    {
        Console.WriteLine("Historique des achats :");
        List<Sale> achats = _dbContext.Achats
            .Include(a => a.Acheteur)
            .Include(a => a.VoitureVendue)
            .OrderBy(a => a.DateAchat)
            .ToList();
 
        foreach (Sale a in achats)
        {
            Console.WriteLine($"{a.DateAchat.ToShortDateString()} : {a.Acheteur.FirstName} {a.Acheteur.LastName} a acheté {a.VoitureVendue.Brand} {a.VoitureVendue.Model}");
        }
    }

    // ajoute un client dans la base de données 
    public void userAddClient()
    {
        Console.Write("Nom : "); string lastName = Console.ReadLine();
        Console.Write("Prénom : "); string firstName = Console.ReadLine();
        Console.Write("Email : "); string email = Console.ReadLine();
        Console.Write("Date de naissance (au format année, mois, date) : ");
        DateTime birthDate = DateTime.SpecifyKind(DateTime.Parse(Console.ReadLine()), DateTimeKind.Utc);
        Console.Write("Téléphone : "); string phone = Console.ReadLine();

        Client client = new Client
        {
            LastName = lastName,
            FirstName = firstName,
            Email = email,
            BirthDate = birthDate,
            PhoneNumber = phone
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
        Console.Write("Marque : "); string brand = Console.ReadLine();
        Console.Write("Modèle : "); string model = Console.ReadLine();
        Console.Write("Année : "); int year = int.Parse(Console.ReadLine());
        Console.Write("Prix HT : "); decimal price = decimal.Parse(Console.ReadLine());
        Console.Write("Couleur : "); string color = Console.ReadLine();

        Car car = new Car
        {
            Brand = brand,
            Model = model,
            Year = year,
            PriceHT = price,
            Color = color,
            Sold = false
        };
        addCar(car);
    }

    public void addCar(Car car)
    {
         
        bool isCarExist = _dbContext.Cars.Any(c => c.Model == car.Model && c.PriceHT == car.PriceHT && c.Brand == car.Brand && c.Year == car.Year && c.Color == car.Color);

        if (!isCarExist){
            _dbContext.Cars.Add(car);
            _dbContext.SaveChanges();
            Console.WriteLine("Voiture ajoutée avec succès.");

        }else 
        {
            Console.WriteLine("Voiture existe deja.");
        }
    }

    // lie client et voiture crée un ACHAT et sauvegarde
    public void Buy()
    {
        Console.Write("Nom du client : "); string lastName = Console.ReadLine();
        List<Client> matchedClients = _dbContext.Clients.Where(c => c.LastName.ToLower() == lastName.ToLower()).ToList();
        Client client; 
        
        if (matchedClients.Count == 0)
        {
            Console.WriteLine("Le client n'est pas trouvé");
            Buy();
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
        Sale sale = new Sale
        {
            Acheteur = client,
            VoitureVendue = car,
            DateAchat = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)
        };
        
        _dbContext.Achats.Add(sale);
        _dbContext.SaveChanges();
        
        Console.WriteLine("Achat effectué avec succès.");
    }

    public void loadCsv()
    {
        string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"../../.."));

        String pathCar = $"{projectRoot}/Data/voitures.csv";

        string[] linesCar = File.ReadAllLines(pathCar);

        for (int i = 1; i < linesCar.Length; i++) // On commence à 1 pour sauter l'en-tête
        {
            string line = linesCar[i];
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
        string[] linesCustomer = File.ReadAllLines(pathClient);

        for (int i = 1; i < linesCustomer.Length; i++) 
        {
            string line = linesCustomer[i];
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
