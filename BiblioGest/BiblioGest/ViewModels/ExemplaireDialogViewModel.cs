using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BiblioGest.Data;
using BiblioGest.Models;
using BiblioGest.Services;

namespace BiblioGest.ViewModels
{
    public class ExemplaireDialogViewModel : INotifyPropertyChanged
    {
        private readonly BiblioGestContext _dbContext;
        private readonly int _livreId;
        private readonly int? _exemplaireId;
        private Exemplaire _exemplaire;

        // Propriétés pour le formulaire
        private string _codeInventaire;
        private string _etat;
        private string _localisation;

        // Erreurs de validation
        private string _errorCodeInventaire;
        private string _errorEtat;
        private string _errorLocalisation;
        private bool _hasErrorCodeInventaire;
        private bool _hasErrorEtat;
        private bool _hasErrorLocalisation;

        // États disponibles pour le ComboBox
        private readonly List<string> _etatsDisponibles = new List<string>
        {
            "Neuf", "Très bon", "Bon", "Acceptable", "Mauvais", "À réparer"
        };

        // Événements
        public event EventHandler CloseRequested;
        public event EventHandler SaveCompleted;
        public event PropertyChangedEventHandler PropertyChanged;

        // Mode du dialogue (Ajout ou Modification)
        private bool IsEditMode => _exemplaireId.HasValue;

        // Constructeur pour un nouvel exemplaire
        public ExemplaireDialogViewModel(int livreId)
        {
            _dbContext = new BiblioGestContext();
            _livreId = livreId;
            _exemplaireId = null;

            // Initialiser un nouvel exemplaire
            _exemplaire = new Exemplaire
            {
                LivreId = _livreId,
                EstDisponible = true,
                Etat = "Bon"  // Valeur par défaut selon le modèle
            };

            // Initialiser les propriétés
            _codeInventaire = string.Empty;
            _etat = "Bon";  // Valeur par défaut selon le modèle
            _localisation = string.Empty;

            // Initialiser les commandes
            SaveCommand = new RelayCommand(_ => Save(), _ => CanSave);
            CancelCommand = new RelayCommand(_ => Cancel());
        }

        // Constructeur pour modifier un exemplaire existant
        public ExemplaireDialogViewModel(int exemplaireId, int livreId)
        {
            _dbContext = new BiblioGestContext();
            _livreId = livreId;
            _exemplaireId = exemplaireId;

            // Charger l'exemplaire existant
            _exemplaire = _dbContext.Exemplaire.Find(exemplaireId);
            if (_exemplaire == null)
            {
                throw new Exception($"L'exemplaire avec l'ID {exemplaireId} n'a pas été trouvé.");
            }

            // Initialiser les propriétés avec les valeurs existantes
            _codeInventaire = _exemplaire.CodeInventaire;
            _etat = _exemplaire.Etat;
            _localisation = _exemplaire.Localisation ?? string.Empty;

            // Initialiser les commandes
            SaveCommand = new RelayCommand(_ => Save(), _ => CanSave);
            CancelCommand = new RelayCommand(_ => Cancel());
        }

        // Propriétés pour le binding dans la vue
        public string WindowTitle => IsEditMode ? "Modifier un exemplaire" : "Ajouter un exemplaire";
        public string FormTitle => IsEditMode ? "Modifier les informations de l'exemplaire" : "Nouvel exemplaire";

        public string CodeInventaire
        {
            get => _codeInventaire;
            set
            {
                if (_codeInventaire != value)
                {
                    _codeInventaire = value;
                    OnPropertyChanged();
                    ValidateCodeInventaire();
                    OnPropertyChanged(nameof(CanSave)); // Notifier que CanSave peut avoir changé
                }
            }
        }

        public string Etat
        {
            get => _etat;
            set
            {
                if (_etat != value)
                {
                    _etat = value;
                    OnPropertyChanged();
                    ValidateEtat();
                    OnPropertyChanged(nameof(CanSave)); // Notifier que CanSave peut avoir changé
                }
            }
        }

        public string Localisation
        {
            get => _localisation;
            set
            {
                if (_localisation != value)
                {
                    _localisation = value;
                    OnPropertyChanged();
                    ValidateLocalisation();
                    OnPropertyChanged(nameof(CanSave)); // Notifier que CanSave peut avoir changé
                }
            }
        }

        public List<string> EtatsDisponibles => _etatsDisponibles;

        // Propriétés d'erreur pour la validation
        public string ErrorCodeInventaire
        {
            get => _errorCodeInventaire;
            set
            {
                if (_errorCodeInventaire != value)
                {
                    _errorCodeInventaire = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool HasErrorCodeInventaire
        {
            get => _hasErrorCodeInventaire;
            set
            {
                if (_hasErrorCodeInventaire != value)
                {
                    _hasErrorCodeInventaire = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }

        public string ErrorEtat
        {
            get => _errorEtat;
            set
            {
                if (_errorEtat != value)
                {
                    _errorEtat = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool HasErrorEtat
        {
            get => _hasErrorEtat;
            set
            {
                if (_hasErrorEtat != value)
                {
                    _hasErrorEtat = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }

        public string ErrorLocalisation
        {
            get => _errorLocalisation;
            set
            {
                if (_errorLocalisation != value)
                {
                    _errorLocalisation = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool HasErrorLocalisation
        {
            get => _hasErrorLocalisation;
            set
            {
                if (_hasErrorLocalisation != value)
                {
                    _hasErrorLocalisation = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }

        // Commandes
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        // Méthodes de validation
        private void ValidateCodeInventaire()
        {
            HasErrorCodeInventaire = false;
            ErrorCodeInventaire = string.Empty;

            if (string.IsNullOrWhiteSpace(CodeInventaire))
            {
                ErrorCodeInventaire = "Le code inventaire est requis.";
                HasErrorCodeInventaire = true;
                return;
            }

            if (CodeInventaire.Length > 50)
            {
                ErrorCodeInventaire = "Le code inventaire ne peut pas dépasser 50 caractères.";
                HasErrorCodeInventaire = true;
                return;
            }

            // Vérifier si le code inventaire existe déjà (sauf pour l'exemplaire en cours d'édition)
            var existingExemplaire = _dbContext.Exemplaire
                .FirstOrDefault(e => e.CodeInventaire == CodeInventaire && 
                                    (!IsEditMode || e.ExemplaireId != _exemplaireId));

            if (existingExemplaire != null)
            {
                ErrorCodeInventaire = "Ce code inventaire est déjà utilisé.";
                HasErrorCodeInventaire = true;
            }
        }

        private void ValidateEtat()
        {
            HasErrorEtat = false;
            ErrorEtat = string.Empty;

            if (string.IsNullOrWhiteSpace(Etat))
            {
                ErrorEtat = "L'état est requis.";
                HasErrorEtat = true;
                return;
            }

            if (Etat.Length > 20)
            {
                ErrorEtat = "L'état ne peut pas dépasser 20 caractères.";
                HasErrorEtat = true;
                return;
            }

            if (!EtatsDisponibles.Contains(Etat))
            {
                ErrorEtat = "Veuillez sélectionner un état valide.";
                HasErrorEtat = true;
            }
        }

        private void ValidateLocalisation()
        {
            HasErrorLocalisation = false;
            ErrorLocalisation = string.Empty;

            if (Localisation?.Length > 100)
            {
                ErrorLocalisation = "La localisation ne peut pas dépasser 100 caractères.";
                HasErrorLocalisation = true;
            }
        }

        // Méthode pour valider toutes les propriétés
        private bool ValidateAll()
        {
            ValidateCodeInventaire();
            ValidateEtat();
            ValidateLocalisation();

            return !HasErrorCodeInventaire && !HasErrorEtat && !HasErrorLocalisation;
        }

        // Propriété pour déterminer si la sauvegarde est possible
        public bool CanSave => 
            !string.IsNullOrWhiteSpace(CodeInventaire) &&
            !string.IsNullOrWhiteSpace(Etat) &&
            !HasErrorCodeInventaire &&
            !HasErrorEtat &&
            !HasErrorLocalisation;

        // Méthode pour enregistrer l'exemplaire
        private void Save()
        {
            try
            {
                // Valider toutes les propriétés
                if (!ValidateAll())
                    return;

                // Mettre à jour les propriétés de l'exemplaire
                _exemplaire.CodeInventaire = CodeInventaire;
                _exemplaire.Etat = Etat;
                _exemplaire.Localisation = Localisation;
                
                // Pour un nouvel exemplaire, définir EstDisponible à true par défaut
                if (!IsEditMode)
                {
                    _exemplaire.EstDisponible = true;
                }

                // Si c'est un nouvel exemplaire, l'ajouter à la base de données
                if (!IsEditMode)
                {
                    _dbContext.Exemplaire.Add(_exemplaire);
                }
                else
                {
                    // Mettre à jour l'exemplaire existant
                    _dbContext.Exemplaire.Update(_exemplaire);
                }

                // Sauvegarder les modifications
                _dbContext.SaveChanges();

                // Notifier que la sauvegarde est terminée
                SaveCompleted?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Erreur lors de l'enregistrement de l'exemplaire: {ex.Message}",
                    "Erreur", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        // Méthode pour annuler l'opération
        private void Cancel()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        // Méthode pour notifier les changements de propriété
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}