using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioGest.Models;

public class Exemplaire
{
    [Key]
    [Column("Id")]
    public int ExemplaireId { get; set; }

    [Required]
    [StringLength(50)]
    [Column("CodeBarre")]
    public string CodeInventaire { get; set; }

    [StringLength(20)]
    public string Etat { get; set; } = "Bon";

    public bool EstDisponible { get; set; } = true;

    [StringLength(100)]
    public string Localisation { get; set; }

    // Relations
    public int LivreId { get; set; }
    [ForeignKey("LivreId")]
    public virtual Livre Livre { get; set; }

    public virtual ICollection<Emprunt> Emprunts { get; set; }
    public virtual ICollection<Reservation> Reservations { get; set; }
}