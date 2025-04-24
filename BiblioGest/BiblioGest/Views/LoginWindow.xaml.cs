using System;
using System.Windows;
using System.Windows.Input; 

// Choisissez un seul namespace cohérent pour tout votre projet
namespace BiblioGest.Views
{
    public partial class LoginWindow : Window
    {
        private readonly Services.AuthenticationService _authService;

        public LoginWindow()
        {
            InitializeComponent();

            try
            {
                _authService = new Services.AuthenticationService();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur avec le service d'authentification : {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void BtnConnexion_Click(object sender, RoutedEventArgs e)
        {
            // Désactiver le bouton pour éviter les doubles clics
            btnConnexion.IsEnabled = false;
            txtErreur.Text = string.Empty;
            
            try
            {
                string identifiant = txtIdentifiant.Text;
                string motDePasse = txtMotDePasse.Password;
                
                // Validation des champs
                if (string.IsNullOrWhiteSpace(identifiant) || string.IsNullOrWhiteSpace(motDePasse))
                {
                    txtErreur.Text = "Veuillez remplir tous les champs.";
                    return;
                }
                
                // Tentative d'authentification
                Models.Bibliothecaire utilisateur = _authService.Authentifier(identifiant, motDePasse);
                
                if (utilisateur != null)
                {
                    // Succès de connexion
                    // Stockage des informations de l'utilisateur connecté dans l'application
                    App.UtilisateurConnecte = utilisateur;
                    
                    // Ouverture de la fenêtre principale et fermeture de la fenêtre de connexion
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    // Échec de connexion
                    txtErreur.Text = "Identifiant ou mot de passe incorrect.";
                    txtMotDePasse.Password = string.Empty;
                    txtMotDePasse.Focus();
                }
            }
            catch (Exception ex)
            {
                txtErreur.Text = $"Erreur lors de la connexion: {ex.Message}";
            }
            finally
            {
                // Réactiver le bouton
                btnConnexion.IsEnabled = true;
            }
        }
    }
}