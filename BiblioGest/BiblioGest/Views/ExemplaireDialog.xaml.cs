using System;
using System.Windows;
using BiblioGest.ViewModels;

namespace BiblioGest.Views
{
    public partial class ExemplaireDialog : Window
    {
        // Constructeur pour XAML Designer
        public ExemplaireDialog()
        {
            InitializeComponent();
            
            System.Diagnostics.Debug.WriteLine("Constructeur ExemplaireDialog() appelé");
        }
        
        // Constructeur pour un nouvel exemplaire
        public ExemplaireDialog(int livreId) : this()
        {
            System.Diagnostics.Debug.WriteLine($"Constructeur ExemplaireDialog(livreId: {livreId}) appelé");
            
            var viewModel = new ExemplaireDialogViewModel(livreId);
            DataContext = viewModel;
            ConfigureViewModel(viewModel);
        }
        
        // Constructeur pour l'édition d'un exemplaire existant
        public ExemplaireDialog(int exemplaireId, int livreId) : this()
        {
            System.Diagnostics.Debug.WriteLine($"Constructeur ExemplaireDialog(exemplaireId: {exemplaireId}, livreId: {livreId}) appelé");
            
            var viewModel = new ExemplaireDialogViewModel(exemplaireId, livreId);
            DataContext = viewModel;
            ConfigureViewModel(viewModel);
        }
        
        private void ConfigureViewModel(ExemplaireDialogViewModel viewModel)
        {
            viewModel.CloseRequested += ViewModel_CloseRequested;
            viewModel.SaveCompleted += ViewModel_SaveCompleted;
        }
        
        private void ViewModel_CloseRequested(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ExemplaireDialog: CloseRequested reçu");
            DialogResult = false;
            Close();
        }
        
        private void ViewModel_SaveCompleted(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ExemplaireDialog: SaveCompleted reçu");
            DialogResult = true;
            Close();
        }
        
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            
            // Nettoyage des abonnements
            if (DataContext is ExemplaireDialogViewModel viewModel)
            {
                viewModel.CloseRequested -= ViewModel_CloseRequested;
                viewModel.SaveCompleted -= ViewModel_SaveCompleted;
            }
            
            System.Diagnostics.Debug.WriteLine("ExemplaireDialog fermée et nettoyée");
        }
    }
}