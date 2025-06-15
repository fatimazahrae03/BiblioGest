using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BiblioGest.Data;
using BiblioGest.Models;
using BiblioGest.Services;
using BiblioGest.Views;
using Microsoft.EntityFrameworkCore;

namespace BiblioGest.ViewModels
{
    public class LoansViewModel : INotifyPropertyChanged
    {
        private string _searchQuery;
        private string _filterStatus;
        private int _currentPage = 1;
        private int _itemsPerPage = 10;
        private int _totalItems;
        private int _totalPages;
        private LoanDetailViewModel _selectedLoan;
        
        // Propriétés pour les statistiques
        public int TodayLoans { get; private set; }
        public int TodayReturns { get; private set; }
        public int TotalActiveLoans { get; private set; }
        public int OverdueLoans { get; private set; }

        public ObservableCollection<LoanDetailViewModel> Loans { get; private set; }

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
            }
        }

        public string FilterStatus
        {
            get => _filterStatus;
            set
            {
                _filterStatus = value;
                OnPropertyChanged(nameof(FilterStatus));
                Search(SearchQuery);
            }
        }

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
                LoadLoans();
            }
        }

        public bool CanGoToPreviousPage => CurrentPage > 1;
        
        public bool CanGoToNextPage => CurrentPage < _totalPages;

        public int TotalPages => _totalPages;

        public LoanDetailViewModel SelectedLoan
        {
            get => _selectedLoan;
            set
            {
                _selectedLoan = value;
                OnPropertyChanged(nameof(SelectedLoan));
            }
        }

        // Commandes
        public ICommand SearchCommand { get; }
        public ICommand NewLoanCommand { get; }
        public ICommand ViewDetailsCommand { get; }
        public ICommand ReturnBookCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }

        public LoansViewModel()
        {
            Loans = new ObservableCollection<LoanDetailViewModel>();
            FilterStatus = "Tous les statuts"; // Valeur par défaut

            // Initialiser les commandes
            SearchCommand = new RelayCommand(Search);
            ViewDetailsCommand = new RelayCommand(ViewLoanDetails);
            ReturnBookCommand = new RelayCommand(ReturnBook);
            PreviousPageCommand = new RelayCommand(PreviousPage, _ => CanGoToPreviousPage);
            NextPageCommand = new RelayCommand(NextPage, _ => CanGoToNextPage);

            // Charger les emprunts
            LoadStatistics();
            LoadLoans();
        }
        

        private void LoadStatistics()
        {
            try
            {
                using (var context = new BiblioGestContext())
                {
                    var today = DateTime.Today;
                    
                    // Emprunts d'aujourd'hui
                    TodayLoans = context.Emprunt.Count(e => e.DateEmprunt.Date == today);
                    
                    // Retours d'aujourd'hui
                    TodayReturns = context.Emprunt.Count(e => e.DateRetourEffective.HasValue && e.DateRetourEffective.Value.Date == today);
                    
                    // Emprunts actifs (non retournés)
                    TotalActiveLoans = context.Emprunt.Count(e => e.DateRetourEffective == null);
                    
                    // Emprunts en retard
                    OverdueLoans = context.Emprunt.Count(e => e.DateRetourEffective == null && e.DateRetourPrevue < DateTime.Now);
                    
                    OnPropertyChanged(nameof(TodayLoans));
                    OnPropertyChanged(nameof(TodayReturns));
                    OnPropertyChanged(nameof(TotalActiveLoans));
                    OnPropertyChanged(nameof(OverdueLoans));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des statistiques : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                
                // Valeurs par défaut en cas d'erreur
                TodayLoans = 0;
                TodayReturns = 0;
                TotalActiveLoans = 0;
                OverdueLoans = 0;
            }
        }

        private void LoadLoans()
        {
            try
            {
                using (var context = new BiblioGestContext())
                {
                    // Requête de base pour obtenir tous les emprunts avec leurs relations
                    var query = context.Emprunt
                        .Include(e => e.Adherent)
                        .Include(e => e.Exemplaire)
                            .ThenInclude(ex => ex.Livre)
                        .Include(e => e.Bibliothecaire)
                        .AsQueryable();

                    // Appliquer le filtre de recherche si nécessaire
                    if (!string.IsNullOrWhiteSpace(SearchQuery))
                    {
                        var searchTerm = SearchQuery.ToLower().Trim();
                        query = query.Where(e => 
                            e.Adherent.Nom.ToLower().Contains(searchTerm) ||
                            e.Adherent.Prenom.ToLower().Contains(searchTerm) ||
                            e.Exemplaire.Livre.Titre.ToLower().Contains(searchTerm) ||
                            e.Exemplaire.Livre.Auteur.ToLower().Contains(searchTerm) ||
                            e.Exemplaire.CodeInventaire.ToLower().Contains(searchTerm));
                    }

                    // Appliquer le filtre de statut
                    switch (FilterStatus)
                    {
                        case "En cours":
                            query = query.Where(e => e.DateRetourEffective == null && e.DateRetourPrevue >= DateTime.Now);
                            break;
                        case "Retournés":
                            query = query.Where(e => e.DateRetourEffective != null);
                            break;
                        case "En retard":
                            query = query.Where(e => e.DateRetourEffective == null && e.DateRetourPrevue < DateTime.Now);
                            break;
                        case "Tous les statuts":
                            // Pas de filtre supplémentaire
                            break;
                        default:
                            // Si on a un statut personnalisé, on filtre directement sur le champ Statut
                            if (!string.IsNullOrEmpty(FilterStatus))
                            {
                                query = query.Where(e => e.Statut == FilterStatus);
                            }
                            break;
                    }

                    // Compter le nombre total d'éléments pour la pagination
                    _totalItems = query.Count();
                    _totalPages = (_totalItems + _itemsPerPage - 1) / _itemsPerPage; // Arrondi vers le haut
                    
                    if (_totalPages < 1) _totalPages = 1;
                    if (_currentPage > _totalPages) _currentPage = _totalPages;
                    
                    OnPropertyChanged(nameof(CanGoToPreviousPage));
                    OnPropertyChanged(nameof(CanGoToNextPage));
                    OnPropertyChanged(nameof(TotalPages));

                    // Récupérer seulement la page courante d'éléments
                    var emprunts = query.OrderByDescending(e => e.DateEmprunt)
                        .Skip((_currentPage - 1) * _itemsPerPage)
                        .Take(_itemsPerPage)
                        .ToList();

                    // Mettre à jour la collection observable
                    Loans.Clear();
                    foreach (var emprunt in emprunts)
                    {
                        Loans.Add(new LoanDetailViewModel
                        {
                            EmpruntId = emprunt.EmpruntId,
                            LivreId = emprunt.Exemplaire.LivreId,
                            TitreLivre = emprunt.Exemplaire.Livre.Titre,
                            AuteurLivre = emprunt.Exemplaire.Livre.Auteur,
                            NomAdherent = $"{emprunt.Adherent.Prenom} {emprunt.Adherent.Nom}",
                            CodeInventaire = emprunt.Exemplaire.CodeInventaire,
                            DateEmprunt = emprunt.DateEmprunt,
                            DateRetourPrevue = emprunt.DateRetourPrevue,
                            DateRetourEffective = emprunt.DateRetourEffective,
                            Statut = emprunt.Statut,
                            Remarques = emprunt.Remarques,
                            NomBibliothecaire = $"{emprunt.Bibliothecaire.Prenom} {emprunt.Bibliothecaire.Nom}",
                            IsReturnVisible = emprunt.DateRetourEffective == null,
                            DetailsBibliotheque = new DetailsBibliothequeViewModel
                            {
                                EtatExemplaire = emprunt.Exemplaire.Etat,
                                LocalisationExemplaire = emprunt.Exemplaire.Localisation,
                                ISBNLivre = emprunt.Exemplaire.Livre.ISBN,
                                AnneeLivre = emprunt.Exemplaire.Livre.Annee,
                                EditeurLivre = emprunt.Exemplaire.Livre.Editeur
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des emprunts : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Search(object parameter)
        {
            CurrentPage = 1; // Retourner à la première page lors d'une recherche
            LoadLoans();
        }

        private void ViewLoanDetails(object parameter)
        {
            var loan = parameter as LoanDetailViewModel;
            if (loan != null)
            {
                try
                {
                    // Afficher les détails de l'emprunt
                    // À implémenter selon vos besoins
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de l'affichage des détails de l'emprunt : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ReturnBook(object parameter)
        {
            var loan = parameter as LoanDetailViewModel;
            if (loan != null)
            {
                try
                {
                    MessageBoxResult result = MessageBox.Show(
                        $"Confirmez-vous le retour du livre '{loan.TitreLivre}' emprunté par {loan.NomAdherent} ?",
                        "Confirmation de retour",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        using (var context = new BiblioGestContext())
                        {
                            var emprunt = context.Emprunt
                                .Include(e => e.Exemplaire)
                                .FirstOrDefault(e => e.EmpruntId == loan.EmpruntId);

                            if (emprunt != null)
                            {
                                // Mettre à jour la date de retour et le statut
                                emprunt.DateRetourEffective = DateTime.Now;
                                emprunt.Statut = "Retourné";
                                
                                // Ajouter le bibliothécaire qui a enregistré le retour (optionnel)
                                // emprunt.BibliothecaireRetourId = Session.CurrentUserId; // À adapter selon votre système d'authentification

                                // Mettre à jour la disponibilité de l'exemplaire
                                var exemplaire = emprunt.Exemplaire;
                                if (exemplaire != null)
                                {
                                    exemplaire.EstDisponible = true;
                                }

                                context.SaveChanges();
                                
                                MessageBox.Show("Le livre a été retourné avec succès.", "Retour enregistré", MessageBoxButton.OK, MessageBoxImage.Information);
                                
                                // Recharger la liste et les statistiques
                                LoadStatistics();
                                LoadLoans();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors du retour du livre : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void PreviousPage(object parameter)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
            }
        }
        
        private void NextPage(object parameter)
        {
            if (CurrentPage < _totalPages)
            {
                CurrentPage++;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Classe pour représenter un emprunt avec des détails complets
    public class LoanDetailViewModel
    {
        public int EmpruntId { get; set; }
        public int LivreId { get; set; }
        public string TitreLivre { get; set; }
        public string AuteurLivre { get; set; }
        public string NomAdherent { get; set; }
        public string CodeInventaire { get; set; }
        public DateTime DateEmprunt { get; set; }
        public DateTime DateRetourPrevue { get; set; }
        public DateTime? DateRetourEffective { get; set; }
        public string Statut { get; set; }
        public string Remarques { get; set; }
        public string NomBibliothecaire { get; set; }
        public bool IsReturnVisible { get; set; }
        public DetailsBibliothequeViewModel DetailsBibliotheque { get; set; }
    }

    // Classe pour représenter des détails supplémentaires sur la bibliothèque
    public class DetailsBibliothequeViewModel
    {
        public string EtatExemplaire { get; set; }
        public string LocalisationExemplaire { get; set; }
        public string ISBNLivre { get; set; }
        public int AnneeLivre { get; set; }
        public string EditeurLivre { get; set; }
    }
}