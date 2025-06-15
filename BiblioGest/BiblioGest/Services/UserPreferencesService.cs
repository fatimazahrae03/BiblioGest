using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace BiblioGest.Services
{
    public class UserPreferencesService
    {
        private readonly string _preferencesFilePath;
        private readonly string _encryptionKey;

        public UserPreferencesService()
        {
            // Définir le chemin du fichier de préférences dans le dossier AppData de l'utilisateur
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string companyFolder = Path.Combine(appDataFolder, "BiblioGest");
            
            // Créer le dossier s'il n'existe pas
            if (!Directory.Exists(companyFolder))
                Directory.CreateDirectory(companyFolder);
                
            _preferencesFilePath = Path.Combine(companyFolder, "userprefs.dat");
            
            // Clé d'encryption - idéalement, elle devrait être stockée de manière sécurisée
            // ou dérivée d'une autre façon
            _encryptionKey = "BiblioGestSecurityKey2025";
        }

        public void SaveUserCredentials(string username, string password)
        {
            try
            {
                // Récupérer la liste existante des identifiants
                var credentialsList = GetAllSavedCredentials();
                
                // Si la liste n'existe pas, la créer
                if (credentialsList == null)
                {
                    credentialsList = new List<UserCredentials>();
                }
                
                // Vérifier si l'utilisateur existe déjà
                var existingUser = credentialsList.FirstOrDefault(c => c.Username == username);
                
                if (existingUser != null)
                {
                    // Mettre à jour le mot de passe de l'utilisateur existant
                    existingUser.Password = EncryptString(password);
                }
                else
                {
                    // Ajouter le nouvel utilisateur
                    credentialsList.Add(new UserCredentials
                    {
                        Username = username,
                        Password = EncryptString(password)
                    });
                }

                // Sauvegarder la liste mise à jour
                SaveCredentialsList(credentialsList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la sauvegarde des identifiants: {ex.Message}");
                // Dans un environnement de production, utilisez un logger approprié
            }
        }

        private void SaveCredentialsList(List<UserCredentials> credentialsList)
        {
            using (var writer = new StreamWriter(_preferencesFilePath))
            {
                var serializer = new XmlSerializer(typeof(List<UserCredentials>));
                serializer.Serialize(writer, credentialsList);
            }
        }

        public List<UserCredentials> GetAllSavedCredentials()
        {
            if (!File.Exists(_preferencesFilePath))
                return new List<UserCredentials>();

            try
            {
                using (var reader = new StreamReader(_preferencesFilePath))
                {
                    var serializer = new XmlSerializer(typeof(List<UserCredentials>));
                    var credentialsList = (List<UserCredentials>)serializer.Deserialize(reader);
                    
                    // Ne pas décrypter les mots de passe ici pour des raisons de sécurité
                    return credentialsList;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement des identifiants: {ex.Message}");
                return new List<UserCredentials>();
            }
        }
        
        public UserCredentials GetLastSavedCredentials()
        {
            var allCredentials = GetAllSavedCredentials();
            if (allCredentials != null && allCredentials.Count > 0)
            {
                var last = allCredentials.LastOrDefault();
                if (last != null)
                {
                    last.Password = DecryptString(last.Password);
                }
                return last;
            }
            return null;
        }

        public string GetPasswordForUsername(string username)
        {
            var credentials = GetAllSavedCredentials();
            var matchingUser = credentials?.FirstOrDefault(c => c.Username == username);
            
            if (matchingUser != null)
            {
                return DecryptString(matchingUser.Password);
            }
            return null;
        }

        public void ClearUserCredentials(string username = null)
        {
            if (username == null)
            {
                // Supprimer tous les identifiants
                if (File.Exists(_preferencesFilePath))
                {
                    try
                    {
                        File.Delete(_preferencesFilePath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur lors de la suppression des identifiants: {ex.Message}");
                    }
                }
            }
            else
            {
                // Supprimer uniquement l'utilisateur spécifié
                var credentialsList = GetAllSavedCredentials();
                if (credentialsList != null && credentialsList.Count > 0)
                {
                    credentialsList = credentialsList.Where(c => c.Username != username).ToList();
                    SaveCredentialsList(credentialsList);
                }
            }
        }

        // Méthodes basiques d'encryption/décryption
        // Note: Pour une application en production, utilisez une méthode plus sécurisée
        private string EncryptString(string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_encryptionKey.PadRight(32, '0').Substring(0, 32));
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }
                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        private string DecryptString(string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_encryptionKey.PadRight(32, '0').Substring(0, 32));
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }

    // Classe pour stocker les identifiants utilisateur
    public class UserCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}