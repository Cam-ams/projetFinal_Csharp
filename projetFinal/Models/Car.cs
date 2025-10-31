﻿using System.ComponentModel.DataAnnotations;

namespace projetFinal.Models;

public class Car
{
    [Key]
    public Guid Id { get; set; } = new Guid();

    [Required] 
    private string brand;
    [Required] 
    private string model;
    [Required] 
    private int year;
    [Required] 
    private decimal priceHT;
    [Required] 
    private string color;
    [Required] 
    private bool sold;

    // Calcul prix TTC avec le prix HT
    public decimal PriceTTC => priceHT * 1.20m;

    // Propriétés
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

    // Relation avec client
    public Guid? ClientId { get; set; }
    public Client? Client { get; set; }
}

