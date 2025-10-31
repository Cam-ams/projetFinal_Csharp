using System.ComponentModel.DataAnnotations;

namespace projetFinal.Models;

public class Client
{
    [Key]
    public Guid Id { get; set; } = new Guid();

    [Required]
    private String firstName;
    
    [Required]
    private String lastName;
    
    [Required]
    private DateTime brirthDate;
    
    [Required]
    private int phoneNumber;
    
    [Required]
    private String email;
    
    

    public string FirstName
    {
        get => firstName;
        set => firstName = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string LastName
    {
        get => lastName;
        set => lastName = value ?? throw new ArgumentNullException(nameof(value));
    }

    public DateTime BrirthDate
    {
        get => brirthDate;
        set => brirthDate = value;
    }

    public int PhoneNumber
    {
        get => phoneNumber;
        set => phoneNumber = value;
    }

    public string Email
    {
        get => email;
        set => email = value ?? throw new ArgumentNullException(nameof(value));
    }
}
//Nom%Prénom%Date de naissance%Téléphone%Email
