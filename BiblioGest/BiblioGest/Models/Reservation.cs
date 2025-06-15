using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioGest.Models;

public class Reservation
{
    [Key]
    [Column("Id")]
    public int ReservationId { get; set; }

    public DateTime DateReservation { get; set; } = DateTime.Now;

    [Column("DateExpiration")]
    public DateTime DateExpiration { get; set; }

    [StringLength(20)]
    public string Statut { get; set; } = "Active";

    // Relations
    public int LivreId { get; set; }
    [ForeignKey("LivreId")]
    public virtual Livre Livre { get; set; }

    public int AdherentId { get; set; }
    [ForeignKey("AdherentId")]
    public virtual Adherent Adherent { get; set; }

    public int BibliothecaireId { get; set; }
    [ForeignKey("BibliothecaireId")]
    public virtual Bibliothecaire Bibliothecaire { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; }
}