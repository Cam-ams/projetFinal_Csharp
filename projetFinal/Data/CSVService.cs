using projetFinal.Models;

namespace projetFinal.Data;

public class CSVService
{
    // Lecture du CSV des voitures
    public List<Car> ReadCars(string path)
    {
        var cars = new List<Car>();
        var lines = File.ReadAllLines(path).Skip(1); // Skip l’en-tête

        foreach (var line in lines)
        {
            var values = line.Split('/');
            cars.Add(new Car
            {
                Brand = values[0],
                Model = values[1],
                Year = int.Parse(values[2]),
                PriceHT = decimal.Parse(values[3]),
                Color = values[4],
                Sold = bool.Parse(values[5])
            });
        }

        return cars;
    }

    // Lecture du CSV des clients
    public List<Client> ReadClients(string path)
    {
        var clients = new List<Client>();
        var lines = File.ReadAllLines(path).Skip(1);

        foreach (var line in lines)
        {
            var values = line.Split('%');
            clients.Add(new Client
            {
                LastName = values[0],
                FirstName = values[1],
                BirthDate = DateTime.Parse(values[2]),
                PhoneNumber = values[3],
                Email = values[4]
            });
        }

        return clients;
    }
}