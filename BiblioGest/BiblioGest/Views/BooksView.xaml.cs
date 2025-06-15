using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BiblioGest.ViewModels;

namespace BiblioGest.Views
{
    public partial class BooksView : UserControl
    {
        public BooksView()
        {
            InitializeComponent();
            // Vérification que le DataContext est défini après l'initialisation
            this.Loaded += BooksView_Loaded;
        }

        private void BooksView_Loaded(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("BooksView chargée, DataContext: " + (DataContext != null ? DataContext.GetType().Name : "null"));
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // Récupérer le ViewModel
                if (DataContext is BooksViewModel viewModel)
                {
                    System.Diagnostics.Debug.WriteLine("Double-clic détecté dans la DataGrid");
                    System.Diagnostics.Debug.WriteLine($"SelectedBook: {(viewModel.SelectedBook != null ? viewModel.SelectedBook.Titre : "null")}");
                    System.Diagnostics.Debug.WriteLine($"ViewBookDetailsCommand peut-il s'exécuter: {viewModel.ViewBookDetailsCommand.CanExecute(null)}");
                    
                    if (viewModel.SelectedBook != null && viewModel.ViewBookDetailsCommand.CanExecute(null))
                    {
                        // Exécuter la commande pour voir les détails
                        System.Diagnostics.Debug.WriteLine("Exécution de ViewBookDetailsCommand");
                        viewModel.ViewBookDetailsCommand.Execute(null);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Impossible d'exécuter ViewBookDetailsCommand - livre non sélectionné ou commande non disponible");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Le DataContext n'est pas un BooksViewModel: " + 
                        (DataContext != null ? DataContext.GetType().Name : "null"));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception lors du double-clic: {ex.Message}");
                MessageBox.Show($"Erreur lors de l'ouverture des détails: {ex.Message}", "Erreur", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}