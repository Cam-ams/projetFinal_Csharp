using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projetFinal.Models;

public class Car
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid(); // Actuellement new Guid()

    [Required] 
    public string brand;

    [Required] 
    public string model;

    [Required] 
    public int year;

    [Required] 
    private decimal priceHT;

    [Required] 
    private string color;

    [Required] 
    private bool sold;

    public decimal PriceTTC => priceHT * 1.20m;

   
   
   
   public string Brand
       {
           get => brand;
           set => brand = value ?? throw new ArgumentNullException(nameof(value));
       }
    
       public string Model
       {
           get => model;
           set => model = value ?? throw new ArgumentNullException(nameof(value));
       }
    
       public int Year
       {
           get => year;
           set => year = value;
       }

       public decimal PriceHT
       {
           get => priceHT;
           set => priceHT = value;
       }

       public string Color
       {
           get => color;
           set => color = value ?? throw new ArgumentNullException(nameof(value));
       }
    
       public bool Sold
       {
           get => sold;
           set => sold = value;
       }
       
       // Relations
       public Guid? ClientId { get; set; }
       public Client? Client { get; set; }
}
