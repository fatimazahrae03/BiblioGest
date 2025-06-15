using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using BiblioGest.Data;
using BiblioGest.Models;
using BiblioGest.Services;
using Microsoft.EntityFrameworkCore;

namespace BiblioGest.ViewModels
{
    public class BookDetailsViewModel : INotifyPropertyChanged
    {
        private readonly BiblioGestContext _dbContext;
        private Livre _livre;
        private ObservableCollection<ExemplaireViewModel> _exemplaires;
        private ExemplaireViewModel _selectedExemplaire;

        public event EventHandler CloseRequested;
        public event EventHandler<int?> ExemplaireDialogRequested;

        public BookDetailsViewModel(int livreId)
        {
            _dbContext = new BiblioGestContext();
            LoadLivreDetails(livreId);
            
            // Commandes
            CloseCommand = new RelayCommand(_ => OnCloseRequested());
            AddExemplaireCommand = new RelayCommand(_ => OnAddExemplaire());
            EditExemplaireCommand = new RelayCommand(_ => OnEditExemplaire(), _ => CanEditOrDeleteExemplaire());
            DeleteExemplaireCommand = new RelayCommand(_ => OnDeleteExemplaire(), _ => CanEditOrDeleteExemplaire());
        }

        private void LoadLivreDetails(int livreId)
        {
            try
            {
                // Charger le livre avec sa catégorie
                Livre = _dbContext.Livre
                    .Include(l => l.Categorie)
                    .FirstOrDefault(l => l.LivreId == livreId);

                if (Livre == null)
                {
                    throw new Exception($"Le livre avec l'ID {livreId} n'a pas été trouvé.");
                }

                // Charger les exemplaires avec leurs emprunts actifs
                var exemplaires = _dbContext.Exemplaire
                    .Where(e => e.LivreId == livreId)
                    .Include(e => e.Emprunts)
                    .ThenInclude(em => em.Adherent)
                    .ToList();

                // Convertir en ViewModels
                Exemplaires = new ObservableCollection<ExemplaireViewModel>(
                    exemplaires.Select(e => new ExemplaireViewModel(e)));

                // Notifier les propriétés dérivées
                OnPropertyChanged(nameof(PublicationInfo));
                OnPropertyChanged(nameof(ExemplairesSummary));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des détails du livre: {ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public Livre Livre
        {
            get { return _livre; }
            set
            {
                _livre = value;
                OnPropertyChanged(nameof(Livre));
            }
        }

        public ObservableCollection<ExemplaireViewModel> Exemplaires
        {
            get { return _exemplaires; }
            set
            {
                _exemplaires = value;
                OnPropertyChanged(nameof(Exemplaires));
            }
        }

        public ExemplaireViewModel SelectedExemplaire
        {
            get { return _selectedExemplaire; }
            set
            {
                _selectedExemplaire = value;
                OnPropertyChanged(nameof(SelectedExemplaire));
                // Mettre à jour la disponibilité des commandes d'édition/suppression
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string PublicationInfo
        {
            get { return $"{Livre?.Editeur} ({Livre?.Annee})"; }
        }

        public string ExemplairesSummary
        {
            get
            {
                if (Exemplaires == null || Exemplaires.Count == 0)
                    return "Aucun exemplaire disponible";

                int disponibles = Exemplaires.Count(e => e.EstDisponible);
                return $"{disponibles} sur {Exemplaires.Count} exemplaires disponibles";
            }
        }

        // Commandes
        public ICommand CloseCommand { get; }
        public ICommand AddExemplaireCommand { get; }
        public ICommand EditExemplaireCommand { get; }
        public ICommand DeleteExemplaireCommand { get; }

        private void OnCloseRequested()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnAddExemplaire()
        {
            // Demander l'ouverture de la fenêtre d'ajout d'exemplaire
            ExemplaireDialogRequested?.Invoke(this, null); // null = nouvel exemplaire
        }

        private void OnEditExemplaire()
        {
            if (SelectedExemplaire != null)
            {
                ExemplaireDialogRequested?.Invoke(this, SelectedExemplaire.ExemplaireId);
            }
        }

        private bool CanEditOrDeleteExemplaire()
        {
            return SelectedExemplaire != null;
        }

        private void OnDeleteExemplaire()
        {
            if (SelectedExemplaire == null) return;

            try
            {
                // Confirmer la suppression
                var result = MessageBox.Show(
                    $"Êtes-vous sûr de vouloir supprimer l'exemplaire avec le code {SelectedExemplaire.CodeInventaire} ?",
                    "Confirmation de suppression",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Vérifier si l'exemplaire est emprunté
                    if (!SelectedExemplaire.EstDisponible)
                    {
                        MessageBox.Show("Impossible de supprimer un exemplaire actuellement emprunté.",
                            "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Récupérer l'exemplaire depuis la base de données
                    var exemplaire = _dbContext.Exemplaire.Find(SelectedExemplaire.ExemplaireId);
                    if (exemplaire != null)
                    {
                        _dbContext.Exemplaire.Remove(exemplaire);
                        _dbContext.SaveChanges();

                        // Mettre à jour la liste
                        Exemplaires.Remove(SelectedExemplaire);
                        OnPropertyChanged(nameof(ExemplairesSummary));

                        MessageBox.Show("L'exemplaire a été supprimé avec succès.",
                            "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la suppression de l'exemplaire: {ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Méthode pour rafraîchir les données après modifications
        public void RefreshExemplaires()
        {
            if (Livre != null)
            {
                LoadLivreDetails(Livre.LivreId);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // ViewModel pour représenter un exemplaire avec des informations sur son emprunt
    public class ExemplaireViewModel : INotifyPropertyChanged
    {
        private readonly Exemplaire _exemplaire;

        public ExemplaireViewModel(Exemplaire exemplaire)
        {
            _exemplaire = exemplaire;
        }

        public int ExemplaireId => _exemplaire.ExemplaireId;
        public string CodeInventaire => _exemplaire.CodeInventaire;
        public string Etat => _exemplaire.Etat;
        public string Localisation => _exemplaire.Localisation;

        // Utilise directement la valeur booléenne EstDisponible de l'exemplaire
        public bool EstDisponible
        {
            get
            {
                // Utiliser directement la propriété EstDisponible du modèle
                return _exemplaire.EstDisponible;
            }
        }

        public string EmpruntInfo
        {
            get
            {
                // Si l'exemplaire est disponible selon la propriété du modèle, ne pas afficher d'info d'emprunt
                if (_exemplaire.EstDisponible)
                    return string.Empty;
                
                // S'il n'est pas disponible, chercher l'emprunt actif
                if (_exemplaire.Emprunts == null)
                    return string.Empty;

                var empruntActif = _exemplaire.Emprunts
                    .FirstOrDefault(e => e.DateRetourEffective == null && e.Statut == "En cours");

                if (empruntActif == null)
                    return string.Empty;

                string retardInfo = string.Empty;
                if (empruntActif.DateRetourPrevue < DateTime.Now)
                {
                    int joursDeRetard = (int)(DateTime.Now - empruntActif.DateRetourPrevue).TotalDays;
                    retardInfo = $" (En retard de {joursDeRetard} jour{(joursDeRetard > 1 ? "s" : "")})";
                }

                return $"Emprunté par {empruntActif.Adherent?.Nom} {empruntActif.Adherent?.Prenom} " +
                       $"jusqu'au {empruntActif.DateRetourPrevue:dd/MM/yyyy}{retardInfo}";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}