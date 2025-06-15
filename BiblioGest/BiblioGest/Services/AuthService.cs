using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BiblioGest.Models;
using BiblioGest.Data;
using System.Security.Cryptography;
using System.Text;

namespace BiblioGest.Services
{
    public class AuthService
    {
        private readonly BiblioGestContext _dbContext;

        public AuthService(BiblioGestContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Bibliothecaire> LoginAsync(string identifiant, string motDePasse)
        {
            try
            {
                // Nettoyer les entrées
                identifiant = identifiant?.Trim();
                
                if (string.IsNullOrEmpty(identifiant) || string.IsNullOrEmpty(motDePasse))
                {
                    return null;
                }

                // Recherche de l'utilisateur par identifiant
                var user = await _dbContext.Bibliothecaire
                    .FirstOrDefaultAsync(b => b.Identifiant == identifiant);

                if (user == null)
                {
                    // Log pour débogage
                    Console.WriteLine($"Aucun utilisateur trouvé avec l'identifiant: {identifiant}");
                    return null;
                }

                // Vérifier si le mot de passe correspond
                // Note: cette méthode doit correspondre à celle utilisée pour créer le hash lors de l'enregistrement
                if (VerifyPassword(motDePasse, user.MotDePasse))
                {
                    return user;
                }
                else
                {
                    // Log pour débogage
                    Console.WriteLine("Mot de passe incorrect");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Log d'erreur
                Console.WriteLine($"Erreur lors de la connexion: {ex.Message}");
                throw;
            }
        }

        // Méthode pour vérifier le mot de passe
        // À adapter selon votre méthode de hachage
        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            // Si vous stockez en texte brut (non recommandé!)
            if (enteredPassword == storedPassword)
                return true;

            // Si vous utilisez un hash, implémentez la vérification appropriée
            // Exemple avec SHA256 (à adapter selon votre méthode)
            /*
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword));
                
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                string hashedEnteredPassword = builder.ToString();
                
                return hashedEnteredPassword.Equals(storedPassword);
            }
            */

            return false; // Retirer cette ligne et décommenter la méthode de vérification appropriée
        }
    }
}