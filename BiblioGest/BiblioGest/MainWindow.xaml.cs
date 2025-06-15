using System.Windows;
using BiblioGest.ViewModels;
using BiblioGest.Models;

namespace BiblioGest.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(Bibliothecaire utilisateurConnecte)
        {
            InitializeComponent();
            
            // Initialiser et définir le contexte de données de la fenêtre
            DataContext = new MainViewModel(utilisateurConnecte, this);
        }
    }
}