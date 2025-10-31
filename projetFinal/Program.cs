// See https://aka.ms/new-console-template for more information

using projetFinal;

using var db = new AppDbContext();
db.Database.EnsureCreated();
 
var insert = new InsertData(db);
insert.SaveFullCars("Data/voitures.csv");
 
Console.WriteLine("Voitures enregistrées en base :");
foreach (var c in db.Cars.ToList())
{
    Console.WriteLine($"{c.Brand} {c.Model} - {c.PriceTTC}€ - {(c.Sold? "Vendue" : "Disponible")}");
    if (c.Sold&& c.Client != null)
        Console.WriteLine($"→ Client : {c.Client.FirstName} {c.Client.LastName} ({c.Client.Email})");