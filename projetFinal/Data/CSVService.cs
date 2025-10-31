using projetFinal.Models;

namespace projetFinal.Data;

public class CSVService
{
    public List<Car> ReadCars(string path)
    {
        var cars = new List<Car>();
        foreach (var line in File.ReadAllLines(path).Skip(1))
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

    public List<Client> ReadClients(string path)
    {
        var clients = new List<Client>();
        foreach (var line in File.ReadAllLines(path).Skip(1))
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