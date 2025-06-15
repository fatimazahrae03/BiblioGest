using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioGest.Models;

public class Categorie
{
    [Key]
    [Column("Id")]
    public int CategorieId { get; set; }

    [Required]
    [StringLength(50)]
    public string Nom { get; set; }

    public string Description { get; set; }

    // Relations
    public virtual ICollection<Livre> Livres { get; set; }
}