using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projetFinal.Models;

public class Concession
{
    [Key]
    public Guid Id { get; set; } = new Guid();
    
    [Required]
    private string name;

    [Required] private List<Car> Cars;
    
    
    [ForeignKey("fk_clients_concession")]
    public Guid ClientConcessionId { get; set; }
    
    [ForeignKey("fk_cars_concession" )]
    public virtual Client CarsConcessionId { get; set; }
}