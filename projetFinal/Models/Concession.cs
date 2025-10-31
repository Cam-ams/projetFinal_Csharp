using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projetFinal.Models;

public class Concession
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    private string name;

    [Required]
    private string address;

    [Required]
    private string phoneNumber;

    // --- Propriétés ---
    public string Name
    {
        get => name;
        set => name = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Address
    {
        get => address;
        set => address = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string PhoneNumber
    {
        get => phoneNumber;
        set => phoneNumber = value ?? throw new ArgumentNullException(nameof(value));
    }

    // --- Relations ---
    public List<Car> Cars { get; set; } = new();
}