using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projetFinal.Models;

public class Client
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    private string lastName;

    [Required]
    private string firstName;

    [Required]
    private DateTime birthDate;

    [Required]
    private string phoneNumber;

    [Required]
    private string email;

    // --- Propriétés ---
    public string LastName
    {
        get => lastName;
        set => lastName = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string FirstName
    {
        get => firstName;
        set => firstName = value ?? throw new ArgumentNullException(nameof(value));
    }

    public DateTime BirthDate
    {
        get => birthDate;
        set => birthDate = value;
    }

    public string PhoneNumber
    {
        get => phoneNumber;
        set => phoneNumber = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Email
    {
        get => email;
        set => email = value ?? throw new ArgumentNullException(nameof(value));
    }

    // --- Relations ---
    public List<Car> Cars { get; set; } = new();
    public List<Achat> Achats { get; set; } = new();
}
