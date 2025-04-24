using System;

namespace BiblioGest.Models
{
    public class Bibliothecaire
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public DateTime DateEmbauche { get; set; }
        public string Poste { get; set; }
        public string Identifiant { get; set; }
        public string MotDePasse { get; set; } // Stocké hashé, jamais en clair
        public string Role { get; set; }
        public DateTime? DerniereConnexion { get; set; }

        // Propriété utile pour l'affichage
        public string NomComplet => $"{Prenom} {Nom}";
    }
}