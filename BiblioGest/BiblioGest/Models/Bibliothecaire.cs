using System;
using System.ComponentModel.DataAnnotations;

namespace BiblioGest.Models
{
    public class Bibliothecaire
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Nom { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Prenom { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Identifiant { get; set; }
        
        [Required]
        [StringLength(100)]
        public string MotDePasse { get; set; }
        
        [StringLength(15)]
        public string Telephone { get; set; }
        
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        
        public string Role { get; set; }
        
        // Nouveaux champs basés sur la table affichée
        public DateTime DateEmbauche { get; set; }
        
        public DateTime? DerniereConnexion { get; set; }
    }
}