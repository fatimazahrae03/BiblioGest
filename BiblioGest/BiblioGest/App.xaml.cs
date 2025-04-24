using System;
using System.Windows;
using BiblioGest.Models;
using BiblioGest.Views;

namespace BiblioGest
{
    public partial class App : Application
    {
        public static Bibliothecaire UtilisateurConnecte { get; set; }
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            try
            {
                // Créer et afficher explicitement la fenêtre de connexion
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur au démarrage : {ex.Message}\n\nDétails : {ex.StackTrace}", 
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }
        
        public App()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Une erreur non gérée s'est produite : {e.Exception.Message}\n\nDétails : {e.Exception.StackTrace}", 
                "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}