using System;
using System.Windows;
using System.Windows.Input;
using BiblioGest.ViewModels;

namespace BiblioGest.Views
{
    public partial class BookDetailsWindow : Window
    {
        // Constructeur sans paramètre pour la compatibilité avec XAML
        public BookDetailsWindow()
        {
            InitializeComponent();
            
            System.Diagnostics.Debug.WriteLine("Constructeur BookDetailsWindow() appelé");
            
            // S'abonner à l'événement DataContextChanged
            DataContextChanged += BookDetailsWindow_DataContextChanged;
        }
        
        // Constructeur avec ViewModel - à utiliser de préférence
        public BookDetailsWindow(BookDetailsViewModel viewModel) : this()
        {
            System.Diagnostics.Debug.WriteLine("Constructeur BookDetailsWindow(viewModel) appelé");
            
            // Définir le DataContext
            DataContext = viewModel;
            
            // L'événement DataContextChanged gérera l'abonnement à CloseRequested
        }
        
        private void BookDetailsWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("DataContext changé dans BookDetailsWindow");
            
            // Désabonner de l'ancien ViewModel si nécessaire
            if (e.OldValue is BookDetailsViewModel oldViewModel)
            {
                System.Diagnostics.Debug.WriteLine("Désabonnement de l'ancien ViewModel");
                oldViewModel.CloseRequested -= ViewModel_CloseRequested;
                oldViewModel.ExemplaireDialogRequested -= ViewModel_ExemplaireDialogRequested;
            }
            
            // S'abonner au nouveau ViewModel
            if (e.NewValue is BookDetailsViewModel newViewModel)
            {
                System.Diagnostics.Debug.WriteLine("Abonnement au nouveau ViewModel");
                newViewModel.CloseRequested += ViewModel_CloseRequested;
                newViewModel.ExemplaireDialogRequested += ViewModel_ExemplaireDialogRequested;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Nouveau DataContext n'est pas un BookDetailsViewModel");
            }
        }
        
        private void ViewModel_CloseRequested(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("CloseRequested reçu, fermeture de la fenêtre");
            Close();
        }
        
        private void ViewModel_ExemplaireDialogRequested(object sender, int? exemplaireId)
        {
            System.Diagnostics.Debug.WriteLine($"ExemplaireDialogRequested reçu, exemplaireId: {exemplaireId}");
            
            if (DataContext is BookDetailsViewModel viewModel && viewModel.Livre != null)
            {
                ExemplaireDialog dialog;
                
                // Création du dialogue en fonction du contexte (ajout ou modification)
                if (exemplaireId.HasValue)
                {
                    // Mode modification
                    dialog = new ExemplaireDialog(exemplaireId.Value, viewModel.Livre.LivreId);
                }
                else
                {
                    // Mode ajout
                    dialog = new ExemplaireDialog(viewModel.Livre.LivreId);
                }
                
                // Définir la fenêtre parent
                dialog.Owner = this;
                
                // Afficher le dialogue
                bool? result = dialog.ShowDialog();
                
                // Si le dialogue a été fermé avec succès (sauvegarde effectuée)
                if (result == true)
                {
                    // Rafraîchir la liste des exemplaires
                    viewModel.RefreshExemplaires();
                }
            }
        }
        
        // Gestionnaire d'événement pour le double-clic sur un exemplaire
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Récupérer l'exemplaire sélectionné
            if (DataContext is BookDetailsViewModel viewModel && viewModel.SelectedExemplaire != null)
            {
                int exemplaireId = viewModel.SelectedExemplaire.ExemplaireId;
                
                // Créer le ViewModel pour l'historique des emprunts
                EmpruntHistoryViewModel historyViewModel = new EmpruntHistoryViewModel(exemplaireId);
                
                // Créer et afficher la fenêtre d'historique des emprunts
                EmpruntHistoryWindow historyWindow = new EmpruntHistoryWindow(historyViewModel);
                historyWindow.Owner = this;
                historyWindow.ShowDialog();
            }
        }
        
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            
            // Nettoyage des abonnements
            if (DataContext is BookDetailsViewModel viewModel)
            {
                viewModel.CloseRequested -= ViewModel_CloseRequested;
                viewModel.ExemplaireDialogRequested -= ViewModel_ExemplaireDialogRequested;
            }
            
            DataContextChanged -= BookDetailsWindow_DataContextChanged;
            
            System.Diagnostics.Debug.WriteLine("BookDetailsWindow fermée et nettoyée");
        }
    }
}