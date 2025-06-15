using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BiblioGest.Models;
using BiblioGest.Services;

namespace BiblioGest.ViewModels
{
    public class AdherentFormViewModel : INotifyPropertyChanged
    {
        private readonly AdherentsService _adherentsService;
        private readonly Window _window;
        private Adherent _adherent;
        private string _windowTitle;
        private StatusItem _selectedStatus;
        private bool _isNew;
        private bool _isLoading;
        private string _errorMessage;
        private bool _hasError;

        public event PropertyChangedEventHandler PropertyChanged;

        // Propriétés
        public Adherent Adherent
        {
            get { return _adherent; }
            set
            {
                _adherent = value;
                OnPropertyChanged(nameof(Adherent));
            }
        }

        public string WindowTitle
        {
            get { return _windowTitle; }
            set
            {
                _windowTitle = value;
                OnPropertyChanged(nameof(WindowTitle));
            }
        }

        public StatusItem SelectedStatus
        {
            get { return _selectedStatus; }
            set
            {
                _selectedStatus = value;
                if (_adherent != null && value != null)
                {
                    _adherent.Statut = value.Value;
                }
                OnPropertyChanged(nameof(SelectedStatus));
            }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                HasError = !string.IsNullOrEmpty(value);
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public bool HasError
        {
            get { return _hasError; }
            set
            {
                _hasError = value;
                OnPropertyChanged(nameof(HasError));
            }
        }

        public ObservableCollection<StatusItem> StatusList { get; private set; }

        // Commandes
        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        // Constructeurs
        public AdherentFormViewModel(Window window, Adherent adherent = null)
        {
            _adherentsService = new AdherentsService();
            _window = window;
            _isNew = adherent == null;

            // Initialiser l'adhérent
            InitializeAdherent(adherent);

            // Définir le titre de la fenêtre
            WindowTitle = _isNew ? "Ajouter un adhérent" : "Modifier un adhérent";

            // Initialiser les statuts
            InitializeStatusList();

            // Initialiser les commandes
            SaveCommand = new RelayCommand(SaveAsync);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void InitializeAdherent(Adherent adherent)
        {
            if (adherent != null)
            {
                // Cloner l'adhérent pour éviter de modifier directement l'original
                Adherent = new Adherent
                {
                    AdherentId = adherent.AdherentId,
                    Nom = adherent.Nom,
                    Prenom = adherent.Prenom,
                    Adresse = adherent.Adresse,
                    Email = adherent.Email,
                    Telephone = adherent.Telephone,
                    DateInscription = adherent.DateInscription,
                    DateFinAdhesion = adherent.DateFinAdhesion,
                    Statut = adherent.Statut
                };
            }
            else
            {
                // Créer un nouvel adhérent
                Adherent = new Adherent
                {
                    DateInscription = DateTime.Now,
                    Statut = "Actif"
                };
            }
        }

        private void InitializeStatusList()
        {
            StatusList = new ObservableCollection<StatusItem>
            {
                new StatusItem { Value = "Actif", Label = "Actif" },
                new StatusItem { Value = "Inactif", Label = "Inactif" },
                new StatusItem { Value = "Suspendu", Label = "Suspendu" }
            };

            // Sélectionner le statut actuel de l'adhérent
            foreach (var status in StatusList)
            {
                if (status.Value == Adherent.Statut)
                {
                    SelectedStatus = status;
                    break;
                }
            }

            // Si aucun statut n'est sélectionné, prendre le premier par défaut
            if (SelectedStatus == null && StatusList.Count > 0)
            {
                SelectedStatus = StatusList[0];
            }
        }

        private async void SaveAsync(object parameter)
        {
            try
            {
                // Valider les données
                if (!ValidateData())
                {
                    return;
                }

                IsLoading = true;
                ErrorMessage = string.Empty;
                bool success;

                // Appliquer le statut sélectionné
                if (SelectedStatus != null)
                {
                    Adherent.Statut = SelectedStatus.Value;
                }

                if (_isNew)
                {
                    // Ajouter un nouvel adhérent
                    success = await _adherentsService.AddAdherentAsync(Adherent);
                }
                else
                {
                    // Mettre à jour un adhérent existant
                    success = await _adherentsService.UpdateAdherentAsync(Adherent);
                }

                if (success)
                {
                    _window.DialogResult = true;
                    _window.Close();
                }
                else
                {
                    ErrorMessage = "Une erreur est survenue lors de l'enregistrement de l'adhérent.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private bool ValidateData()
        {
            // Vérifier que les champs obligatoires sont remplis
            if (string.IsNullOrWhiteSpace(Adherent.Nom))
            {
                ErrorMessage = "Le nom est obligatoire.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Adherent.Prenom))
            {
                ErrorMessage = "Le prénom est obligatoire.";
                return false;
            }

            if (Adherent.DateInscription == DateTime.MinValue)
            {
                ErrorMessage = "La date d'inscription est obligatoire.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Adherent.Statut))
            {
                ErrorMessage = "Le statut est obligatoire.";
                return false;
            }

            // Vérifier que la date de fin d'adhésion est postérieure à la date d'inscription
            if (Adherent.DateFinAdhesion.HasValue && Adherent.DateFinAdhesion.Value < Adherent.DateInscription)
            {
                ErrorMessage = "La date de fin d'adhésion doit être postérieure à la date d'inscription.";
                return false;
            }

            // Vérifier le format de l'email (validation simple)
            if (!string.IsNullOrWhiteSpace(Adherent.Email) && !IsValidEmail(Adherent.Email))
            {
                ErrorMessage = "Le format de l'email est incorrect.";
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void Cancel(object parameter)
        {
            _window.DialogResult = false;
            _window.Close();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}