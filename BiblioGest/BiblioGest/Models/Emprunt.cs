using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioGest.Models;

public class Emprunt
{
    [Key]
    [Column("Id")]
    public int EmpruntId { get; set; }

    public DateTime DateEmprunt { get; set; } = DateTime.Now;

    public DateTime DateRetourPrevue { get; set; }

    [Column("DateRetourEffective")]
    public DateTime? DateRetourEffective { get; set; }

    [Column("Statut")]
    public string Statut { get; set; } = "En cours";

    public string Remarques { get; set; }

    // Relations
    public int AdherentId { get; set; }
    [ForeignKey("AdherentId")]
    public virtual Adherent Adherent { get; set; }

    public int ExemplaireId { get; set; }
    [ForeignKey("ExemplaireId")]
    public virtual Exemplaire Exemplaire { get; set; }

    public int BibliothecaireId { get; set; }
    [ForeignKey("BibliothecaireId")]
    public virtual Bibliothecaire Bibliothecaire { get; set; }

    public int? BibliothecaireRetourId { get; set; }
    [ForeignKey("BibliothecaireRetourId")]
    public virtual Bibliothecaire BibliothecaireRetour { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; }
}