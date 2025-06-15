using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioGest.Models;

public class Livre
{
    [Key]
    [Column("Id")]
    public int LivreId { get; set; }

    [Required]
    [StringLength(100)]
    public string Titre { get; set; }

    [Required]
    [StringLength(100)]
    public string Auteur { get; set; }

    [StringLength(13)]
    public string ISBN { get; set; }

    [Column("AnneePublication")]
    public int Annee { get; set; }

    [StringLength(100)]
    public string Editeur { get; set; }

    [Column("Description")]
    [StringLength(500)]
    public string Resume { get; set; }

    public int NombreExemplaires { get; set; } = 0;

    public DateTime DateAjout { get; set; } = DateTime.Now;

    public bool EstDisponible { get; set; } = true;

    [StringLength(255)]
    public string ImageCouverture { get; set; }

    // Relations
    public int CategorieId { get; set; }
    [ForeignKey("CategorieId")]
    public virtual Categorie Categorie { get; set; }

    public virtual ICollection<Exemplaire> Exemplaires { get; set; }
}