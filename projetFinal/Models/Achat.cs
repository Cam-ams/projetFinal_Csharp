using System.ComponentModel.DataAnnotations;

namespace projetFinal.Models;

public class Achat
{
    [Key]
    public Guid Id { get; set; } = new Guid();

    [Required] 
    private DateTime dateAchat;

    public DateTime DateAchat
    {
        get => dateAchat;
        set => dateAchat = value;
    }

    // Relations pour achat devant mise en lien CLIENT - VOITURE
    [Required] 
    public Guid ClientId { get; set; }
    public Client Acheteur { get; set; }

    [Required] 
    public Guid CarId { get; set; }
    public Car VoitureVendue { get; set; }
}