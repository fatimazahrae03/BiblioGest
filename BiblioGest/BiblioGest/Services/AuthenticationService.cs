using System;
using System.Configuration;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using BiblioGest.Models;

namespace BiblioGest.Services
{
    public class AuthenticationService
    {
        private readonly DatabaseConnection _db;
        
        // Dans AuthenticationService.cs
        public AuthenticationService()
        {
            try
            {
                // Explicitement vérifier si la chaîne de connexion existe
                string connectionString = ConfigurationManager.ConnectionStrings["bibliogest"]?.ConnectionString;
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception("La chaîne de connexion 'bibliogest' n'est pas définie dans App.config");
                }
                _db = new DatabaseConnection(connectionString);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur d'initialisation du service d'authentification : {ex.Message}", ex);
            }
        }
        // Méthode d'authentification
        public Bibliothecaire Authentifier(string identifiant, string motDePasse)
        {
            // Recherche du bibliothécaire par identifiant
            string query = "SELECT * FROM Bibliothecaire WHERE Identifiant = @identifiant";
            
            var parameters = new System.Collections.Generic.Dictionary<string, object>
            {
                {"identifiant", identifiant}
            };
            
            DataTable result = _db.ExecuteQuery(query, parameters);
            
            // Vérification si l'utilisateur existe
            if (result.Rows.Count == 0)
                return null;
                
            DataRow userData = result.Rows[0];
            
            // Récupération du mot de passe hashé de la BDD
            string motDePasseHash = userData["MotDePasse"].ToString();
            
            // Vérification du mot de passe
            if (VerifierMotDePasse(motDePasse, motDePasseHash))
            {
                // Mise à jour de la dernière connexion
                MettreAJourDerniereConnexion(Convert.ToInt32(userData["Id"]));
                
                // Retourne les données du bibliothécaire
                return new Bibliothecaire
                {
                    Id = Convert.ToInt32(userData["Id"]),
                    Nom = userData["Nom"].ToString(),
                    Prenom = userData["Prenom"].ToString(),
                    Email = userData["Email"].ToString(),
                    Telephone = userData["Telephone"].ToString(),
                    DateEmbauche = Convert.ToDateTime(userData["DateEmbauche"]),
                    Poste = userData["Poste"].ToString(),
                    Identifiant = userData["Identifiant"].ToString(),
                    Role = userData["Role"].ToString(),
                    DerniereConnexion = userData["DerniereConnexion"] != DBNull.Value 
                        ? Convert.ToDateTime(userData["DerniereConnexion"]) 
                        : (DateTime?)null
                };
            }
            
            return null;
        }
        
        // Mise à jour de la date de dernière connexion
        private void MettreAJourDerniereConnexion(int id)
        {
            string query = "UPDATE Bibliothecaire SET DerniereConnexion = @date WHERE Id = @id";
            
            var parameters = new System.Collections.Generic.Dictionary<string, object>
            {
                {"id", id},
                {"date", DateTime.Now}
            };
            
            _db.ExecuteNonQuery(query, parameters);
        }
        
        // Création d'un hash sécurisé pour le mot de passe (à utiliser lors de la création d'un compte)
        public string HashMotDePasse(string motDePasse)
        {
            // On utilise SHA256 pour le hachage avec sel
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Ajout d'un "sel" aléatoire
                string sel = GenererSel();
                
                // Combinaison du mot de passe et du sel
                string combined = motDePasse + sel;
                
                // Conversion en bytes
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(combined));
                
                // Conversion en chaîne hexadécimale
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                
                // Format: [hash]:[sel]
                return $"{builder.ToString()}:{sel}";
            }
        }
        
        // Vérification du mot de passe
        private bool VerifierMotDePasse(string motDePasse, string motDePasseStocke)
        {
            // Séparation du hash et du sel
            string[] parts = motDePasseStocke.Split(':');
            if (parts.Length != 2)
                return false;
                
            string hashStocke = parts[0];
            string sel = parts[1];
            
            // Recréation du hash avec le mot de passe fourni et le sel stocké
            using (SHA256 sha256Hash = SHA256.Create())
            {
                string combined = motDePasse + sel;
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(combined));
                
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                
                string hashCalcule = builder.ToString();
                
                // Comparaison des hash
                return hashStocke.Equals(hashCalcule);
            }
        }
        
        // Génération d'un sel aléatoire
        private string GenererSel()
        {
            byte[] bytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }
            return Convert.ToBase64String(bytes);
        }
        
        // Méthode pour créer un nouvel utilisateur
        public bool CreerBibliothecaire(Bibliothecaire bibliothecaire)
        {
            // Hash du mot de passe avant stockage
            string motDePasseHash = HashMotDePasse(bibliothecaire.MotDePasse);
            
            string query = @"INSERT INTO Bibliothecaire 
                          (Nom, Prenom, Email, Telephone, DateEmbauche, Poste, Identifiant, MotDePasse, Role) 
                          VALUES 
                          (@nom, @prenom, @email, @telephone, @dateEmbauche, @poste, @identifiant, @motDePasse, @role)";
            
            var parameters = new System.Collections.Generic.Dictionary<string, object>
            {
                {"nom", bibliothecaire.Nom},
                {"prenom", bibliothecaire.Prenom},
                {"email", bibliothecaire.Email},
                {"telephone", bibliothecaire.Telephone},
                {"dateEmbauche", bibliothecaire.DateEmbauche},
                {"poste", bibliothecaire.Poste},
                {"identifiant", bibliothecaire.Identifiant},
                {"motDePasse", motDePasseHash},
                {"role", bibliothecaire.Role ?? "Bibliothécaire"}
            };
            
            int result = _db.ExecuteNonQuery(query, parameters);
            return result > 0;
        }
    }
}