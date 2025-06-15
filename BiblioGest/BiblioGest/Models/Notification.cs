using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioGest.Models;

public class Notification
{
    [Key]
    [Column("Id")]
    public int NotificationId { get; set; }

    [Required]
    [StringLength(50)]
    public string Type { get; set; }

    [Column("Message")]
    public string Message { get; set; }

    public DateTime DateCreation { get; set; } = DateTime.Now;

    public DateTime? DateEnvoi { get; set; }

    public bool EstLue { get; set; } = false;

    // Relations
    public int AdherentId { get; set; }
    [ForeignKey("AdherentId")]
    public virtual Adherent Adherent { get; set; }

    public int? EmpruntId { get; set; }
    [ForeignKey("EmpruntId")]
    public virtual Emprunt Emprunt { get; set; }

    public int BibliothecaireId { get; set; }
    [ForeignKey("BibliothecaireId")]
    public virtual Bibliothecaire Bibliothecaire { get; set; }
}