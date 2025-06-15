using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using BiblioGest.Data;
using BiblioGest.Models;
using BiblioGest.Services;
using BiblioGest.Views;
using Microsoft.EntityFrameworkCore;

namespace BiblioGest.ViewModels
{
    public class BooksViewModel : INotifyPropertyChanged
    {
        private readonly int _itemsPerPage = 10;
        private int _currentPage = 1;
        private int _totalPages;
        private string _searchText = string.Empty;
        private string _selectedFilter = "Tous";
        private ObservableCollection<Livre> _displayedBooks;
        private List<Livre> _allBooks;
        private Livre _selectedBook;
        private readonly BiblioGestContext _dbContext;

        public BooksViewModel()
        {
            // Initialiser avec la base de données
            _dbContext = new BiblioGestContext();
            LoadBooksFromDatabase();
            DisplayedBooks = new ObservableCollection<Livre>();

            // Charger les catégories depuis la base de données pour les filtres
            LoadFilterOptions();

            // Charger la première page
            CalculateTotalPages();
            LoadCurrentPage();

            // Initialiser les commandes
            NextPageCommand = new RelayCommand(NextPage, CanGoToNextPage);
            PreviousPageCommand = new RelayCommand(PreviousPage, CanGoToPreviousPage);
            SearchCommand = new RelayCommand(Search);
            AddBookCommand = new RelayCommand(AddBook);
            EditBookCommand = new RelayCommand(EditBook, CanEditOrDeleteBook);
            DeleteBookCommand = new RelayCommand(DeleteBook, CanEditOrDeleteBook);
            ViewBookDetailsCommand = new RelayCommand(ViewBookDetails, CanViewBookDetails);
        }

        private void LoadBooksFromDatabase()
        {
            try
            {
                // Charger tous les livres avec leurs catégories
                _allBooks = _dbContext.Livre.Include("Categorie").ToList();
        
                // Message de debug pour voir combien de livres ont été chargés
                System.Diagnostics.Debug.WriteLine($"Livres chargés: {_allBooks.Count}");
        
                // Si aucun livre n'est trouvé, afficher un message
                if (_allBooks.Count == 0)
                {
                    System.Windows.MessageBox.Show("Aucun livre n'a été trouvé dans la base de données.", 
                        "Information", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Erreur lors du chargement des livres: {ex.Message}", "Erreur",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                _allBooks = new List<Livre>();
            }
        }

        private void LoadFilterOptions()
        {
            try
            {
                // Charger toutes les catégories pour les filtres
                var categories = _dbContext.Categorie.Select(c => c.Nom).ToList();
                FilterOptions = new ObservableCollection<string>(new[] { "Tous" }.Concat(categories));
            }
            catch (Exception)
            {
                // Fallback en cas d'erreur
                FilterOptions = new ObservableCollection<string> { "Tous" };
            }
        }

        public ObservableCollection<Livre> DisplayedBooks
        {
            get { return _displayedBooks; }
            set
            {
                _displayedBooks = value;
                OnPropertyChanged(nameof(DisplayedBooks));
            }
        }

        public ObservableCollection<string> FilterOptions { get; set; }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        public string SelectedFilter
        {
            get { return _selectedFilter; }
            set
            {
                _selectedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));
                Search(null);
            }
        }

        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
                OnPropertyChanged(nameof(CurrentPageDisplay));
            }
        }

        public string CurrentPageDisplay => $"Page {CurrentPage} sur {TotalPages}";

        public int TotalPages
        {
            get { return _totalPages; }
            set
            {
                _totalPages = value;
                OnPropertyChanged(nameof(TotalPages));
                OnPropertyChanged(nameof(CurrentPageDisplay));
            }
        }

        public Livre SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                _selectedBook = value;
                OnPropertyChanged(nameof(SelectedBook));
                CommandManager.InvalidateRequerySuggested(); // Pour mettre à jour CanExecute des commandes
            }
        }

        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand AddBookCommand { get; }
        public ICommand EditBookCommand { get; }
        public ICommand DeleteBookCommand { get; }
        public ICommand ViewBookDetailsCommand { get; }

        private void CalculateTotalPages()
        {
            var filteredBooks = ApplyFilters();
            TotalPages = (int)Math.Ceiling(filteredBooks.Count / (double)_itemsPerPage);

            // S'assurer que la page actuelle est valide
            if (CurrentPage > TotalPages && TotalPages > 0)
            {
                CurrentPage = TotalPages;
            }
            else if (TotalPages == 0)
            {
                CurrentPage = 1;
            }
        }

        private List<Livre> ApplyFilters()
        {
            var results = _allBooks;

            // Appliquer le filtre de catégorie si ce n'est pas "Tous"
            if (_selectedFilter != "Tous")
            {
                results = results.Where(l => l.Categorie?.Nom == _selectedFilter).ToList();
            }

            // Appliquer le filtre de recherche par texte
            if (!string.IsNullOrWhiteSpace(_searchText))
            {
                string searchLower = _searchText.ToLower();
                results = results.Where(l =>
                    l.Titre.ToLower().Contains(searchLower) ||
                    l.Auteur.ToLower().Contains(searchLower) ||
                    l.ISBN?.ToLower().Contains(searchLower) == true ||
                    l.Resume?.ToLower().Contains(searchLower) == true
                ).ToList();
            }

            return results;
        }

        private void LoadCurrentPage()
        {
            var filteredBooks = ApplyFilters();

            // Calculer les indices de début et de fin pour la pagination
            int startIndex = (CurrentPage - 1) * _itemsPerPage;
            var pagedBooks = filteredBooks
                .Skip(startIndex)
                .Take(_itemsPerPage)
                .ToList();

            // Mettre à jour la collection observable
            DisplayedBooks.Clear();
            foreach (var book in pagedBooks)
            {
                DisplayedBooks.Add(book);
            }
        }

        private void NextPage(object parameter)
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                LoadCurrentPage();
            }
        }

        private bool CanGoToNextPage(object parameter)
        {
            return CurrentPage < TotalPages;
        }

        private void PreviousPage(object parameter)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadCurrentPage();
            }
        }

        private bool CanGoToPreviousPage(object parameter)
        {
            return CurrentPage > 1;
        }

        public void Search(object parameter)
        {
            CurrentPage = 1; // Revenir à la première page lors d'une nouvelle recherche
            CalculateTotalPages();
            LoadCurrentPage();
        }

        private void AddBook(object parameter)
        {
            try
            {
                // Capture l'instance actuelle de MainWindow pour l'utiliser comme Owner
                var mainWindow = System.Windows.Application.Current.MainWindow;
                
                // Crée une nouvelle fenêtre pour l'ajout de livre
                var window = new BookFormWindow(null, RefreshBooks);
                
                // Configure la fenêtre pour qu'elle soit modale par rapport à la fenêtre principale
                window.Owner = mainWindow;
                window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
                
                // Affiche la fenêtre de manière modale
                window.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Erreur lors de l'ouverture de la fenêtre d'ajout: {ex.Message}\n\nStack Trace: {ex.StackTrace}", 
                    "Erreur", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void EditBook(object parameter)
        {
            try
            {
                if (SelectedBook != null)
                {
                    // Capture l'instance actuelle de MainWindow pour l'utiliser comme Owner
                    var mainWindow = System.Windows.Application.Current.MainWindow;
                    
                    // Crée une nouvelle fenêtre pour l'édition de livre
                    var window = new BookFormWindow(SelectedBook, RefreshBooks);
                    
                    // Configure la fenêtre pour qu'elle soit modale par rapport à la fenêtre principale
                    window.Owner = mainWindow;
                    window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
                    
                    // Affiche la fenêtre de manière modale
                    window.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Erreur lors de l'ouverture de la fenêtre d'édition: {ex.Message}\n\nStack Trace: {ex.StackTrace}", 
                    "Erreur", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
        
        private bool CanViewBookDetails(object parameter)
        {
            return SelectedBook != null;
        }

       private void ViewBookDetails(object parameter)
{
    try
    {
        // Obtenez le livre à partir du paramètre ou utilisez SelectedBook
        Livre livre = parameter as Livre ?? SelectedBook;
        
        if (livre != null)
        {
            System.Diagnostics.Debug.WriteLine($"Ouverture des détails pour le livre: {livre.Titre} (ID: {livre.LivreId})");
            
            // Capture l'instance actuelle de MainWindow pour l'utiliser comme Owner
            var mainWindow = System.Windows.Application.Current.MainWindow;
            if (mainWindow == null)
            {
                System.Diagnostics.Debug.WriteLine("MainWindow est null, impossible de définir Owner");
            }
            
            // Crée une instance de la fenêtre de détails du livre
            var viewModel = new BookDetailsViewModel(livre.LivreId);
            
            // Option 1: Utiliser le constructeur avec le ViewModel directement
            var window = new BookDetailsWindow(viewModel);
            
            /* Option 2: Utiliser le constructeur par défaut et définir le DataContext
            var window = new BookDetailsWindow();
            window.DataContext = viewModel;
            */
            
            // Configure la fenêtre pour qu'elle soit modale par rapport à la fenêtre principale
            window.Owner = mainWindow;
            window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            
            System.Diagnostics.Debug.WriteLine("Fenêtre de détails créée, affichage...");
            
            // Affiche la fenêtre de manière modale
            window.ShowDialog();
            
            System.Diagnostics.Debug.WriteLine("Fenêtre de détails fermée");
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("Aucun livre sélectionné pour afficher les détails");
        }
    }
    catch (Exception ex)
    {
        System.Diagnostics.Debug.WriteLine($"Exception dans ViewBookDetails: {ex.Message}\n{ex.StackTrace}");
        System.Windows.MessageBox.Show($"Erreur lors de l'ouverture des détails du livre: {ex.Message}", 
            "Erreur", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
    }
}
        
        public void RefreshBooks()
        {
            // Recharger les livres depuis la base de données
            LoadBooksFromDatabase();
    
            // Recalculer la pagination et recharger la page courante
            CalculateTotalPages();
            LoadCurrentPage();
        }

        private bool CanEditOrDeleteBook(object parameter)
        {
            return SelectedBook != null;
        }

        private void DeleteBook(object parameter)
        {
            if (SelectedBook != null)
            {
                var result = System.Windows.MessageBox.Show(
                    $"Êtes-vous sûr de vouloir supprimer le livre \"{SelectedBook.Titre}\" ?",
                    "Confirmation de suppression",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Question);

                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    try
                    {
                        // Supprimer le livre de la base de données
                        _dbContext.Livre.Remove(SelectedBook);
                        _dbContext.SaveChanges();

                        // Mettre à jour la liste
                        _allBooks.Remove(SelectedBook);
                        CalculateTotalPages();
                        LoadCurrentPage();

                        System.Windows.MessageBox.Show("Le livre a été supprimé avec succès.", "Succès",
                            System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show($"Erreur lors de la suppression : {ex.Message}", "Erreur",
                            System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}