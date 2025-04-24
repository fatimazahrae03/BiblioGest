using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using BiblioGest.Models;
using BiblioGest.Services;

namespace BiblioGest.Repositories
{
    public class BibliothecaireRepository
    {
        private readonly DatabaseConnection _db;
        private readonly AuthenticationService _authService;
        
        public BibliothecaireRepository()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["bibliogest"].ConnectionString;
            _db = new DatabaseConnection(connectionString);
            _authService = new AuthenticationService();
        }
        
        // Obtenir tous les bibliothécaires
        public List<Bibliothecaire> GetAllBibliothecaires()
        {
            List<Bibliothecaire> bibliothecaires = new List<Bibliothecaire>();
            string query = "SELECT * FROM Bibliothecaire ORDER BY Nom, Prenom";
            
            DataTable result = _db.ExecuteQuery(query);
            
            foreach (DataRow row in result.Rows)
            {
                bibliothecaires.Add(new Bibliothecaire
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Nom = row["Nom"].ToString(),
                    Prenom = row["Prenom"].ToString(),
                    Email = row["Email"].ToString(),
                    Telephone = row["Telephone"].ToString(),
                    DateEmbauche = Convert.ToDateTime(row["DateEmbauche"]),
                    Poste = row["Poste"].ToString(),
                    Identifiant = row["Identifiant"].ToString(),
                    Role = row["Role"].ToString(),
                    DerniereConnexion = row["DerniereConnexion"] != DBNull.Value 
                        ? Convert.ToDateTime(row["DerniereConnexion"]) 
                        : (DateTime?)null
                });
            }
            
            return bibliothecaires;
        }
        
        // Obtenir un bibliothécaire par ID
        public Bibliothecaire GetBibliothecaireById(int id)
        {
            string query = "SELECT * FROM Bibliothecaire WHERE Id = @id";
            
            var parameters = new Dictionary<string, object>
            {
                {"id", id}
            };
            
            DataTable result = _db.ExecuteQuery(query, parameters);
            
            if (result.Rows.Count == 0)
                return null;
                
            DataRow row = result.Rows[0];
            
            return new Bibliothecaire
            {
                Id = Convert.ToInt32(row["Id"]),
                Nom = row["Nom"].ToString(),
                Prenom = row["Prenom"].ToString(),
                Email = row["Email"].ToString(),
                Telephone = row["Telephone"].ToString(),
                DateEmbauche = Convert.ToDateTime(row["DateEmbauche"]),
                Poste = row["Poste"].ToString(),
                Identifiant = row["Identifiant"].ToString(),
                Role = row["Role"].ToString(),
                DerniereConnexion = row["DerniereConnexion"] != DBNull.Value 
                    ? Convert.ToDateTime(row["DerniereConnexion"]) 
                    : (DateTime?)null
            };
        }
        
        // Ajouter un bibliothécaire
        public bool AjouterBibliothecaire(Bibliothecaire bibliothecaire)
        {
            // Utilisation du service d'authentification pour le hachage du mot de passe
            return _authService.CreerBibliothecaire(bibliothecaire);
        }
        
        // Mettre à jour un bibliothécaire sans changer son mot de passe
        public bool MettreAJourBibliothecaire(Bibliothecaire bibliothecaire)
        {
            string query = @"UPDATE Bibliothecaire 
                          SET Nom = @nom, 
                              Prenom = @prenom, 
                              Email = @email, 
                              Telephone = @telephone, 
                              DateEmbauche = @dateEmbauche, 
                              Poste = @poste, 
                              Role = @role
                          WHERE Id = @id";
            
            var parameters = new Dictionary<string, object>
            {
                {"id", bibliothecaire.Id},
                {"nom", bibliothecaire.Nom},
                {"prenom", bibliothecaire.Prenom},
                {"email", bibliothecaire.Email},
                {"telephone", bibliothecaire.Telephone},
                {"dateEmbauche", bibliothecaire.DateEmbauche},
                {"poste", bibliothecaire.Poste},
                {"role", bibliothecaire.Role}
            };
            
            return _db.ExecuteNonQuery(query, parameters) > 0;
        }
        
        // Mettre à jour le mot de passe d'un bibliothécaire
        public bool MettreAJourMotDePasse(int id, string nouveauMotDePasse)
        {
            string motDePasseHash = _authService.HashMotDePasse(nouveauMotDePasse);
            
            string query = "UPDATE Bibliothecaire SET MotDePasse = @motDePasse WHERE Id = @id";
            
            var parameters = new Dictionary<string, object>
            {
                {"id", id},
                {"motDePasse", motDePasseHash}
            };
            
            return _db.ExecuteNonQuery(query, parameters) > 0;
        }
        
        // Supprimer un bibliothécaire
        public bool SupprimerBibliothecaire(int id)
        {
            string query = "DELETE FROM Bibliothecaire WHERE Id = @id";
            
            var parameters = new Dictionary<string, object>
            {
                {"id", id}
            };
            
            return _db.ExecuteNonQuery(query, parameters) > 0;
        }
        
        // Vérifier si un identifiant existe déjà
        public bool IdentifiantExiste(string identifiant, int? idExclusion = null)
        {
            string query = "SELECT COUNT(*) FROM Bibliothecaire WHERE Identifiant = @identifiant";
            
            var parameters = new Dictionary<string, object>
            {
                {"identifiant", identifiant}
            };
            
            // Si on fournit un ID d'exclusion (pour les mises à jour)
            if (idExclusion.HasValue)
            {
                query += " AND Id != @idExclusion";
                parameters.Add("idExclusion", idExclusion.Value);
            }
            
            var result = _db.ExecuteScalar(query, parameters);
            return Convert.ToInt32(result) > 0;
        }
        
        // Vérifier si un email existe déjà
        public bool EmailExiste(string email, int? idExclusion = null)
        {
            if (string.IsNullOrEmpty(email))
                return false;
                
            string query = "SELECT COUNT(*) FROM Bibliothecaire WHERE Email = @email";
            
            var parameters = new Dictionary<string, object>
            {
                {"email", email}
            };
            
            // Si on fournit un ID d'exclusion (pour les mises à jour)
            if (idExclusion.HasValue)
            {
                query += " AND Id != @idExclusion";
                parameters.Add("idExclusion", idExclusion.Value);
            }
            
            var result = _db.ExecuteScalar(query, parameters);
            return Convert.ToInt32(result) > 0;
        }
    }
}