using System.Windows;
using System.Windows.Controls;
using BiblioGest.Services;
using System.Linq;

namespace BiblioGest.Views
{
    public partial class LoginWindow : Window
    {
        private readonly UserPreferencesService _preferencesService;
        
        public LoginWindow()
        {
            InitializeComponent();
            
            _preferencesService = new UserPreferencesService();
            
            // Pour lier le PasswordBox (qui n'a pas de binding de données normal)
            PasswordBox.PasswordChanged += PasswordBox_PasswordChanged;
            
            // Initialiser le mot de passe depuis le ViewModel lorsque la fenêtre est chargée
            Loaded += LoginWindow_Loaded;
        }

        private void LoginWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.LoginViewModel viewModel)
            {
                // Remplir la ComboBox avec les identifiants sauvegardés
                LoadSavedUsernames();
                
                // Définir le mot de passe si disponible
                if (!string.IsNullOrEmpty(viewModel.MotDePasse))
                {
                    PasswordBox.Password = viewModel.MotDePasse;
                }
            }
        }
        
        private void LoadSavedUsernames()
        {
            // Récupérer tous les identifiants sauvegardés
            var savedCredentials = _preferencesService.GetAllSavedCredentials();
            if (savedCredentials != null && savedCredentials.Count > 0)
            {
                // Remplir la ComboBox avec les noms d'utilisateur
                UserComboBox.ItemsSource = savedCredentials.Select(c => c.Username).ToList();
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.LoginViewModel viewModel)
            {
                viewModel.MotDePasse = ((PasswordBox)sender).Password;
            }
        }
    }
}