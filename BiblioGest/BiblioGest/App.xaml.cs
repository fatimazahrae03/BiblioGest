using System;
using System.Windows;
using BiblioGest.Data;
using BiblioGest.Views;
using Microsoft.EntityFrameworkCore;

namespace BiblioGest
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialisation de la base de données
            try
            {
                using (var dbContext = new BiblioGestContext())
                {
                    // Créer la base de données si elle n'existe pas
                    dbContext.Database.EnsureCreated();
                    
                    // Ou pour appliquer les migrations (si vous utilisez des migrations)
                    // dbContext.Database.Migrate();
                }
                
                // Démarrer l'application avec la fenêtre de login
                var loginWindow = new LoginWindow();
                loginWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la connexion à la base de données: {ex.Message}", 
                    "Erreur de démarrage", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
                Shutdown();
            }
        }
    }
}