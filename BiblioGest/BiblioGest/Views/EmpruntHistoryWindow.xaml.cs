using System;
using System.Windows;
using BiblioGest.ViewModels;

namespace BiblioGest.Views
{
    public partial class EmpruntHistoryWindow : Window
    {
        // Constructeur sans paramètre pour la compatibilité avec XAML
        public EmpruntHistoryWindow()
        {
            InitializeComponent();
            DataContextChanged += EmpruntHistoryWindow_DataContextChanged;
        }
        
        // Constructeur avec ViewModel - à utiliser de préférence
        public EmpruntHistoryWindow(EmpruntHistoryViewModel viewModel) : this()
        {
            DataContext = viewModel;
            // L'événement DataContextChanged gérera l'abonnement à CloseRequested
        }
        
        private void EmpruntHistoryWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Désabonner de l'ancien ViewModel si nécessaire
            if (e.OldValue is EmpruntHistoryViewModel oldViewModel)
            {
                oldViewModel.CloseRequested -= ViewModel_CloseRequested;
            }
            
            // S'abonner au nouveau ViewModel
            if (e.NewValue is EmpruntHistoryViewModel newViewModel)
            {
                newViewModel.CloseRequested += ViewModel_CloseRequested;
            }
        }
        
        private void ViewModel_CloseRequested(object sender, EventArgs e)
        {
            Close();
        }
        
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            
            // Nettoyage des abonnements
            if (DataContext is EmpruntHistoryViewModel viewModel)
            {
                viewModel.CloseRequested -= ViewModel_CloseRequested;
            }
            
            DataContextChanged -= EmpruntHistoryWindow_DataContextChanged;
        }
    }
}