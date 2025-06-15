using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using BiblioGest.Data;
using BiblioGest.Models;
using BiblioGest.Services;
using Microsoft.EntityFrameworkCore;

namespace BiblioGest.ViewModels
{
    public class EmpruntHistoryViewModel : INotifyPropertyChanged
    {
        private readonly BiblioGestContext _dbContext;
        private ObservableCollection<EmpruntViewModel> _emprunts;
        private Exemplaire _exemplaire;

        public event EventHandler CloseRequested;

        public EmpruntHistoryViewModel(int exemplaireId)
        {
            _dbContext = new BiblioGestContext();
            
            // Commandes
            CloseCommand = new RelayCommand(_ => OnCloseRequested());
            
            // Charger les données
            LoadEmpruntHistory(exemplaireId);
        }

        private void LoadEmpruntHistory(int exemplaireId)
        {
            try
            {
                // Charger l'exemplaire avec son livre et tous ses emprunts
                _exemplaire = _dbContext.Exemplaire
                    .Include(e => e.Livre)
                    .Include(e => e.Emprunts)
                        .ThenInclude(em => em.Adherent)
                    .FirstOrDefault(e => e.ExemplaireId == exemplaireId);

                if (_exemplaire == null)
                {
                    throw new Exception($"L'exemplaire avec l'ID {exemplaireId} n'a pas été trouvé.");
                }

                // Convertir les emprunts en ViewModels et les trier par date d'emprunt (le plus récent en premier)
                Emprunts = new ObservableCollection<EmpruntViewModel>(
                    _exemplaire.Emprunts
                        .Select(e => new EmpruntViewModel(e))
                        .OrderByDescending(e => e.DateEmprunt));

                // Mettre à jour l'en-tête
                OnPropertyChanged(nameof(HeaderInfo));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Erreur lors du chargement de l'historique des emprunts: {ex.Message}",
                    "Erreur", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        public string HeaderInfo
        {
            get
            {
                if (_exemplaire == null)
                    return string.Empty;

                return $"Exemplaire: {_exemplaire.CodeInventaire} | Livre: {_exemplaire.Livre?.Titre}";
            }
        }

        public ObservableCollection<EmpruntViewModel> Emprunts
        {
            get { return _emprunts; }
            set
            {
                _emprunts = value;
                OnPropertyChanged(nameof(Emprunts));
            }
        }

        // Commandes
        public ICommand CloseCommand { get; }

        private void OnCloseRequested()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // ViewModel pour représenter un emprunt avec des informations additionnelles
    public class EmpruntViewModel
    {
        private readonly Emprunt _emprunt;

        public EmpruntViewModel(Emprunt emprunt)
        {
            _emprunt = emprunt;
        }

        public int EmpruntId => _emprunt.EmpruntId;
        public DateTime DateEmprunt => _emprunt.DateEmprunt;
        public DateTime DateRetourPrevue => _emprunt.DateRetourPrevue;
        public DateTime? DateRetourEffective => _emprunt.DateRetourEffective;
        public string Statut => _emprunt.Statut;
        public string Remarques => _emprunt.Remarques;

        public string AdherentNom => $"{_emprunt.Adherent?.Nom} {_emprunt.Adherent?.Prenom}";
    }
}