using System.ComponentModel.DataAnnotations;
namespace projetFinal.Models;


public class Client
{
    [Key]
    public Guid Id { get; set; } = new Guid();

    [Required] 
    private string _lastName;
    [Required] 
    private string _firstName;
    [Required] 
    private DateTime _birthDate;
    [Required] 
    private string _phoneNumber;
    [Required] 
    private string _email;

    // Propriétés
    public string LastName
    {
        get => _lastName;
        set => _lastName = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string FirstName
    {
        get => _firstName;
        set => _firstName = value ?? throw new ArgumentNullException(nameof(value));
    }

    public DateTime BirthDate
    {
        get => _birthDate;
        set => _birthDate = value;
    }

    public string PhoneNumber
    {
        get => _phoneNumber;
        set => _phoneNumber = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Email
    {
        get => _email;
        set => _email = value ?? throw new ArgumentNullException(nameof(value));
    }

    // Relations autres tables 
    public List<Car> Cars { get; set; } = new();
    public List<Sale> Sales { get; set; } = new(); 
}