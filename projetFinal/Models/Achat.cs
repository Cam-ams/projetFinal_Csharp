using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projetFinal.Models;

public class Achat
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    private DateTime dateAchat;

    // --- Propriétés ---
    public DateTime DateAchat
    {
        get => dateAchat;
        set => dateAchat = value;
    }

    // --- Relations ---
    [Required]
    public Guid ClientId { get; set; }
    public Client Acheteur { get; set; }

    [Required]
    public Guid CarId { get; set; }
    public Car VoitureVendue { get; set; }
}