using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BiblioGest.Models;
using BiblioGest.Services;
using BiblioGest.Views;

namespace BiblioGest.ViewModels
{
    public class AdherentsViewModel : INotifyPropertyChanged
    {
        private readonly AdherentsService _adherentsService;
        private string _searchText;
        private Adherent _selectedAdherent;
        private ObservableCollection<Adherent> _adherents;
        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _totalItems;
        private StatusItem _selectedStatus;
        private bool _isLoading;

        // Propriétés publiques
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        public ObservableCollection<Adherent> Adherents
        {
            get { return _adherents; }
            set
            {
                _adherents = value;
                OnPropertyChanged(nameof(Adherents));
            }
        }

        public Adherent SelectedAdherent
        {
            get { return _selectedAdherent; }
            set
            {
                _selectedAdherent = value;
                OnPropertyChanged(nameof(SelectedAdherent));
            }
        }

        public ObservableCollection<StatusItem> StatusList { get; private set; }

        public StatusItem SelectedStatus
        {
            get { return _selectedStatus; }
            set
            {
                _selectedStatus = value;
                OnPropertyChanged(nameof(SelectedStatus));
            }
        }

        public string PageInfo
        {
            get { return $"Page {_currentPage} / {Math.Ceiling((double)_totalItems / _pageSize)}"; }
        }

        public bool CanGoToPreviousPage => _currentPage > 1;
        public bool CanGoToNextPage => _currentPage < Math.Ceiling((double)_totalItems / _pageSize);

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        // Commandes
        public ICommand SearchCommand { get; private set; }
        public ICommand AddCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand DetailsCommand { get; private set; }
        public ICommand NextPageCommand { get; private set; }
        public ICommand PreviousPageCommand { get; private set; }
        public ICommand ShowEmpruntsCommand { get; private set; }

     
       

        // Constructeur
        public AdherentsViewModel()
        {
            // Initialiser le service
            _adherentsService = new AdherentsService();

            // Initialiser les commandes
            SearchCommand = new RelayCommand(Search);
            AddCommand = new RelayCommand(AddAdherent);
            EditCommand = new RelayCommand(EditAdherent);
            DeleteCommand = new RelayCommand(DeleteAdherent);
            DetailsCommand = new RelayCommand(ShowDetails);
            NextPageCommand = new RelayCommand(NextPage, param => CanGoToNextPage);
            PreviousCommandPreview = new RelayCommand(PreviousPage, param => CanGoToPreviousPage);
           

            // Initialiser les filtres de statut
            InitializeStatusFilters();

            // Initialiser la collection d'adhérents
            Adherents = new ObservableCollection<Adherent>();

            // Charger les adhérents
            LoadAdherentsAsync();
        }

        public RelayCommand PreviousCommandPreview { get; set; }

        private void InitializeStatusFilters()
        {
            StatusList = new ObservableCollection<StatusItem>
            {
                new StatusItem { Value = "", Label = "Tous les statuts" },
                new StatusItem { Value = "Actif", Label = "Actif" },
                new StatusItem { Value = "Inactif", Label = "Inactif" },
                new StatusItem { Value = "Suspendu", Label = "Suspendu" }
            };

            SelectedStatus = StatusList[0]; // Par défaut, tous les statuts
        }

        public void Search(object parameter)
        {
            _currentPage = 1;
            LoadAdherentsAsync();
        }

        private async Task LoadAdherentsAsync()
        {
            try
            {
                IsLoading = true;

                // Récupérer les adhérents depuis la base de données
                var result = await _adherentsService.GetAdherentsAsync(
                    SearchText,
                    SelectedStatus?.Value,
                    _currentPage,
                    _pageSize);

                // Mettre à jour la collection d'adhérents
                Adherents = new ObservableCollection<Adherent>(result.adherents);

                // Mettre à jour le nombre total d'éléments
                _totalItems = result.totalCount;

                // Mettre à jour les informations de pagination
                OnPropertyChanged(nameof(PageInfo));
                OnPropertyChanged(nameof(CanGoToPreviousPage));
                OnPropertyChanged(nameof(CanGoToNextPage));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue lors du chargement des adhérents : {ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async void AddAdherent(object parameter)
        {
            var formWindow = new AdherentFormView();
            // Suppression de la définition de Owner pour éviter l'erreur

            bool? result = formWindow.ShowDialog();

            if (result == true)
            {
                // L'adhérent a été ajouté, rafraîchir la liste
                await LoadAdherentsAsync();
            }
        }

        private async void EditAdherent(object parameter)
        {
            if (parameter is Adherent adherent)
            {
                var formWindow = new AdherentFormView(adherent);
                // Suppression de la définition de Owner pour éviter l'erreur

                bool? result = formWindow.ShowDialog();

                if (result == true)
                {
                    // L'adhérent a été modifié, rafraîchir la liste
                    await LoadAdherentsAsync();
                }
            }
        }

        private async void DeleteAdherent(object parameter)
        {
            if (parameter is Adherent adherent)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Êtes-vous sûr de vouloir supprimer l'adhérent {adherent.Prenom} {adherent.Nom} ?",
                    "Confirmation de suppression",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Supprimer l'adhérent
                        bool success = await _adherentsService.DeleteAdherentAsync(adherent.AdherentId);

                        if (success)
                        {
                            MessageBox.Show("Adhérent supprimé avec succès !",
                                "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                            // Recharger les adhérents
                            LoadAdherentsAsync();
                        }
                        else
                        {
                            MessageBox.Show("Impossible de supprimer l'adhérent.",
                                "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Une erreur est survenue lors de la suppression de l'adhérent : {ex.Message}",
                            "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void ShowDetails(object parameter)
        {
            if (parameter is Adherent adherent)
            {
                MessageBox.Show($"Détails de l'adhérent {adherent.Prenom} {adherent.Nom}\n" +
                                $"Email: {adherent.Email}\n" +
                                $"Téléphone: {adherent.Telephone}\n" +
                                $"Adresse: {adherent.Adresse}\n" +
                                $"Date d'inscription: {adherent.DateInscription:dd/MM/yyyy}\n" +
                                $"Fin d'adhésion: {adherent.DateFinAdhesion:dd/MM/yyyy}\n" +
                                $"Statut: {adherent.Statut}",
                    "Détails de l'adhérent",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        

        private void NextPage(object parameter)
        {
            if (CanGoToNextPage)
            {
                _currentPage++;
                LoadAdherentsAsync();
            }
        }

        private void PreviousPage(object parameter)
        {
            if (CanGoToPreviousPage)
            {
                _currentPage--;
                LoadAdherentsAsync();
            }
        }

        // Implémentation de INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class StatusItem
    {
        public string Value { get; set; }
        public string Label { get; set; }
    }
}