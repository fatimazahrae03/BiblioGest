using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using BiblioGest.Data;
using BiblioGest.Services;
using BiblioGest.Views;
using System.Configuration;

namespace BiblioGest.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly AuthService _authService;
        private readonly UserPreferencesService _preferencesService;
        private string _identifiant;
        private string _motDePasse;
        private string _errorMessage;
        private bool _sesouvenir;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Identifiant
        {
            get { return _identifiant; }
            set
            {
                _identifiant = value;
                OnPropertyChanged(nameof(Identifiant));
                
                // Charger le mot de passe dès que l'identifiant est saisi
                if (!string.IsNullOrEmpty(value))
                {
                    LoadPassword();
                }
            }
        }

        public string MotDePasse
        {
            get { return _motDePasse; }
            set
            {
                _motDePasse = value;
                OnPropertyChanged(nameof(MotDePasse));
            }
        }

        public bool SeSouvenir
        {
            get { return _sesouvenir; }
            set
            {
                _sesouvenir = value;
                OnPropertyChanged(nameof(SeSouvenir));
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            var dbContext = new BiblioGestContext();
            _authService = new AuthService(dbContext);
            _preferencesService = new UserPreferencesService();
            
            // Charger les préférences utilisateur
            LoadUserPreferences();
            
            // Ajouter des logs pour le débogage
            Console.WriteLine($"LoginViewModel initialisé. Identifiant: {Identifiant}, Mot de passe présent: {!string.IsNullOrEmpty(MotDePasse)}");
            
            LoginCommand = new RelayCommand(async param =>
            {
                try
                {
                    var user = await _authService.LoginAsync(Identifiant, MotDePasse);
                    if (user != null)
                    {
                        // Sauvegarder les identifiants si l'option est cochée
                        if (SeSouvenir)
                        {
                            SaveUserCredentials();
                        }
                        else
                        {
                            // Si l'option a été décochée, effacer uniquement les identifiants de cet utilisateur
                            _preferencesService.ClearUserCredentials(Identifiant);
                        }
                        
                        // Utilisateur connecté avec succès
                        MessageBox.Show($"Bienvenue {user.Prenom} {user.Nom} !", "Connexion réussie");
                        
                        // Ouvrir la fenêtre principale et fermer la fenêtre de connexion
                        var mainWindow = new MainWindow(user);
                        mainWindow.Show();
                        
                        // Fermer la fenêtre de connexion
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window is LoginWindow)
                            {
                                window.Close();
                                break;
                            }
                        }
                    }
                    else
                    {
                        ErrorMessage = "Identifiant ou mot de passe incorrect.";
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur de connexion: {ex.Message}";
                }
            });
        }

        private void LoadUserPreferences()
        {
            // Charger les dernières informations d'identification sauvegardées
            var savedCredentials = _preferencesService.GetLastSavedCredentials();
            if (savedCredentials != null)
            {
                Identifiant = savedCredentials.Username;
                MotDePasse = savedCredentials.Password;
                SeSouvenir = true;
            }
        }

        private void LoadPassword()
        {
            // Vérifier si un mot de passe est enregistré pour cet identifiant
            var savedPassword = _preferencesService.GetPasswordForUsername(Identifiant);
            if (!string.IsNullOrEmpty(savedPassword))
            {
                _motDePasse = savedPassword; // Mettre à jour le champ directement
                
                // Mettre à jour la case à cocher si un mot de passe est trouvé
                if (!SeSouvenir)
                {
                    SeSouvenir = true;
                }
                
                // Notifier le changement de mot de passe pour que l'UI se mette à jour
                OnPropertyChanged(nameof(MotDePasse));
                
                Console.WriteLine($"Mot de passe chargé pour l'identifiant: {Identifiant}");
            }
        }

        private void SaveUserCredentials()
        {
            _preferencesService.SaveUserCredentials(Identifiant, MotDePasse);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}