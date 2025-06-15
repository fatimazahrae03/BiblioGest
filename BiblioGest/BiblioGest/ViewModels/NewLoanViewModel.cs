using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BiblioGest.Data;
using BiblioGest.Models;
using BiblioGest.Services;
using BiblioGest.Views;
using Microsoft.EntityFrameworkCore;

namespace BiblioGest.ViewModels
{
    public class NewLoanViewModel : INotifyPropertyChanged
    {
        #region Properties

        private string _adherentSearchQuery;
        private string _livreSearchQuery;
        private ObservableCollection<Adherent> _adherents;
        private ObservableCollection<Livre> _livres;
        private ObservableCollection<Exemplaire> _exemplairesDisponibles;
        private Adherent _selectedAdherent;
        private Livre _selectedLivre;
        private Exemplaire _selectedExemplaire;
        private DateTime _dateEmprunt;
        private DateTime _dateRetourPrevue;
        private string _remarques;
        private bool _canSave;

        public string AdherentSearchQuery
        {
            get => _adherentSearchQuery;
            set
            {
                _adherentSearchQuery = value;
                OnPropertyChanged(nameof(AdherentSearchQuery));
            }
        }

        public string LivreSearchQuery
        {
            get => _livreSearchQuery;
            set
            {
                _livreSearchQuery = value;
                OnPropertyChanged(nameof(LivreSearchQuery));
            }
        }

        public ObservableCollection<Adherent> Adherents
        {
            get => _adherents;
            set
            {
                _adherents = value;
                OnPropertyChanged(nameof(Adherents));
            }
        }

        public ObservableCollection<Livre> Livres
        {
            get => _livres;
            set
            {
                _livres = value;
                OnPropertyChanged(nameof(Livres));
            }
        }

        public ObservableCollection<Exemplaire> ExemplairesDisponibles
        {
            get => _exemplairesDisponibles;
            set
            {
                _exemplairesDisponibles = value;
                OnPropertyChanged(nameof(ExemplairesDisponibles));
            }
        }

        public Adherent SelectedAdherent
        {
            get => _selectedAdherent;
            set
            {
                _selectedAdherent = value;
                OnPropertyChanged(nameof(SelectedAdherent));
                OnPropertyChanged(nameof(IsAdherentSelected));
                UpdateCanSave();
            }
        }

        public Livre SelectedLivre
        {
            get => _selectedLivre;
            set
            {
                _selectedLivre = value;
                OnPropertyChanged(nameof(SelectedLivre));
                OnPropertyChanged(nameof(IsLivreSelected));
                LoadExemplairesDisponibles();
                UpdateCanSave();
            }
        }

        public Exemplaire SelectedExemplaire
        {
            get => _selectedExemplaire;
            set
            {
                _selectedExemplaire = value;
                OnPropertyChanged(nameof(SelectedExemplaire));
                UpdateCanSave();
            }
        }

        public DateTime DateEmprunt
        {
            get => _dateEmprunt;
            set
            {
                _dateEmprunt = value;
                OnPropertyChanged(nameof(DateEmprunt));
                
                // Ajuster la date de retour par défaut (+21 jours)
                if (_dateRetourPrevue == default || (_dateRetourPrevue - _dateEmprunt).TotalDays <= 0)
                {
                    DateRetourPrevue = _dateEmprunt.AddDays(21);
                }
            }
        }

        public DateTime DateRetourPrevue
        {
            get => _dateRetourPrevue;
            set
            {
                _dateRetourPrevue = value;
                OnPropertyChanged(nameof(DateRetourPrevue));
            }
        }

        public string Remarques
        {
            get => _remarques;
            set
            {
                _remarques = value;
                OnPropertyChanged(nameof(Remarques));
            }
        }

        public bool IsAdherentSelected => SelectedAdherent != null;
        public bool IsLivreSelected => SelectedLivre != null;

        public bool CanSave
        {
            get => _canSave;
            private set
            {
                _canSave = value;
                OnPropertyChanged(nameof(CanSave));
            }
        }

        #endregion

        #region Commands

        public ICommand SearchAdherentCommand { get; }
        public ICommand SearchLivreCommand { get; }
        public ICommand SaveLoanCommand { get; }
        public ICommand CancelCommand { get; }

        #endregion

        // Référence à la fenêtre principale pour fermer la vue actuelle
        private Window _parentWindow;
        // Event à déclencher lorsqu'un nouvel emprunt est créé
        public event Action EmpruntCreated;

        public NewLoanViewModel(Window parentWindow)
        {
            _parentWindow = parentWindow;
            
            // Initialisation des collections
            Adherents = new ObservableCollection<Adherent>();
            Livres = new ObservableCollection<Livre>();
            ExemplairesDisponibles = new ObservableCollection<Exemplaire>();
            
            // Initialisation des dates par défaut
            DateEmprunt = DateTime.Today;
            DateRetourPrevue = DateTime.Today.AddDays(21);
            
            // Initialisation des commandes
            SearchAdherentCommand = new RelayCommand(SearchAdherent);
            SearchLivreCommand = new RelayCommand(SearchLivre);
            SaveLoanCommand = new RelayCommand(SaveLoan, _ => CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void SearchAdherent(object parameter)
        {
            try
            {
                using (var context = new BiblioGestContext())
                {
                    var query = context.Adherent.AsQueryable();

                    if (!string.IsNullOrWhiteSpace(AdherentSearchQuery))
                    {
                        var searchTerm = AdherentSearchQuery.ToLower().Trim();
                        query = query.Where(a => 
                            a.Nom.ToLower().Contains(searchTerm) ||
                            a.Prenom.ToLower().Contains(searchTerm) ||
                            a.Email.ToLower().Contains(searchTerm));
                    }

                    // Filtrer uniquement les adhérents actifs
                    query = query.Where(a => a.Statut == "Actif" && (!a.DateFinAdhesion.HasValue || a.DateFinAdhesion >= DateTime.Today));

                    var results = query.Take(20).ToList();
                    
                    Adherents.Clear();
                    foreach (var adherent in results)
                    {
                        Adherents.Add(adherent);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la recherche d'adhérents : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchLivre(object parameter)
        {
            try
            {
                using (var context = new BiblioGestContext())
                {
                    var query = context.Livre
                        .Include(l => l.Exemplaires)
                        .AsQueryable();

                    if (!string.IsNullOrWhiteSpace(LivreSearchQuery))
                    {
                        var searchTerm = LivreSearchQuery.ToLower().Trim();
                        query = query.Where(l => 
                            l.Titre.ToLower().Contains(searchTerm) ||
                            l.Auteur.ToLower().Contains(searchTerm) ||
                            l.ISBN.Contains(searchTerm));
                    }

                    // Récupérer uniquement les livres ayant au moins un exemplaire disponible
                    query = query.Where(l => l.Exemplaires.Any(e => e.EstDisponible));

                    var results = query.Take(20).ToList();
                    
                    Livres.Clear();
                    foreach (var livre in results)
                    {
                        livre.NombreExemplaires = livre.Exemplaires.Count(e => e.EstDisponible);
                        Livres.Add(livre);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la recherche de livres : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadExemplairesDisponibles()
        {
            if (SelectedLivre == null)
            {
                ExemplairesDisponibles.Clear();
                return;
            }

            try
            {
                using (var context = new BiblioGestContext())
                {
                    var exemplaires = context.Exemplaire
                        .Where(e => e.LivreId == SelectedLivre.LivreId && e.EstDisponible)
                        .OrderBy(e => e.CodeInventaire)
                        .ToList();

                    ExemplairesDisponibles.Clear();
                    foreach (var exemplaire in exemplaires)
                    {
                        ExemplairesDisponibles.Add(exemplaire);
                    }

                    // Sélectionner le premier exemplaire si disponible
                    if (ExemplairesDisponibles.Count > 0)
                    {
                        SelectedExemplaire = ExemplairesDisponibles[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des exemplaires : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveLoan(object parameter)
        {
            try
            {
                if (SelectedAdherent == null || SelectedExemplaire == null)
                {
                    MessageBox.Show("Veuillez sélectionner un adhérent et un exemplaire.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (DateEmprunt > DateTime.Today)
                {
                    MessageBox.Show("La date d'emprunt ne peut pas être dans le futur.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (DateRetourPrevue <= DateEmprunt)
                {
                    MessageBox.Show("La date de retour prévue doit être postérieure à la date d'emprunt.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var context = new BiblioGestContext())
                {
                    // Récupérer l'adhérent et l'exemplaire pour s'assurer qu'ils sont à jour
                    var adherent = context.Adherent.Find(SelectedAdherent.AdherentId);
                    var exemplaire = context.Exemplaire.Find(SelectedExemplaire.ExemplaireId);

                    if (adherent == null)
                    {
                        MessageBox.Show("L'adhérent sélectionné n'existe plus dans la base de données.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (exemplaire == null)
                    {
                        MessageBox.Show("L'exemplaire sélectionné n'existe plus dans la base de données.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (!exemplaire.EstDisponible)
                    {
                        MessageBox.Show("L'exemplaire sélectionné n'est plus disponible.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Créer le nouvel emprunt
                    var nouvelEmprunt = new Emprunt
                    {
                        AdherentId = SelectedAdherent.AdherentId,
                        ExemplaireId = SelectedExemplaire.ExemplaireId,
                        DateEmprunt = DateEmprunt,
                        DateRetourPrevue = DateRetourPrevue,
                        Statut = "En cours",
                        Remarques = Remarques,
                         // Supposant que vous avez un service de session
                    };

                    // Marquer l'exemplaire comme non disponible
                    exemplaire.EstDisponible = false;

                    // Ajouter et sauvegarder
                    context.Emprunt.Add(nouvelEmprunt);
                    context.SaveChanges();

                    MessageBox.Show("L'emprunt a été enregistré avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    // Fermer la vue et rafraîchir la liste des emprunts
                    EmpruntCreated?.Invoke();
                    Cancel(null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'enregistrement de l'emprunt : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel(object parameter)
        {
            _parentWindow?.Close();
        }

        private void UpdateCanSave()
        {
            CanSave = SelectedAdherent != null && SelectedExemplaire != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}