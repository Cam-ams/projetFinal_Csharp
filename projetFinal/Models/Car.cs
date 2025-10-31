using System.ComponentModel.DataAnnotations;

namespace projetFinal.Models;

public class Car
{
   [Key]
    public Guid Id { get; set; } = new Guid();
   
   [Required]
    private String brand ;

   [Required] 
   private String model;

   [Required] 
   private int year;

   [Required]
   private float priceHT;

   [Required] 
   private string color;

   [Required] 
   private Boolean sold;

   public float PriceTTC => PriceHT * 1.2f;
   
   
   
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
    
       public float PriceHT
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
       
       // Relations A MODIF PLUS TARD
       public int? ClientId { get; set; }
       public Client? Client { get; set; }
 
       public int ConcessionId { get; set; }
       public Concession Concession { get; set; }
       
}

