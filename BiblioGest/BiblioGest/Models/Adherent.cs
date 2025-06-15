using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioGest.Models;

public class Adherent
{
    [Key]
    [Column("Id")]
    public int AdherentId { get; set; }

    [Required]
    [StringLength(50)]
    public string Nom { get; set; }

    [Required]
    [StringLength(50)]
    public string Prenom { get; set; }

    [StringLength(255)]
    public string Adresse { get; set; }

    [StringLength(100)]
    public string Email { get; set; }

    [StringLength(20)]
    public string Telephone { get; set; }

    public DateTime DateInscription { get; set; } = DateTime.Now;

    public DateTime? DateFinAdhesion { get; set; }

    [StringLength(20)]
    public string Statut { get; set; } = "Actif";

    // Relations
    public virtual ICollection<Emprunt> Emprunts { get; set; }
    public virtual ICollection<Reservation> Reservations { get; set; }
    public virtual ICollection<Notification> Notifications { get; set; }
}