using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using BiblioGest.Models;
using BiblioGest.Services;
using BiblioGest.Views;

namespace BiblioGest.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly Window _window;
        private string _currentViewTitle;
        private DateTime _currentDateTime;
        private bool _isDashboardVisible;
        private bool _isBooksVisible;
        private bool _isMembersVisible; // Nouvelle propriété pour la vue des adhérents
        private bool _isLoansVisible;
        private DispatcherTimer _timer;

        // Propriétés pour les statistiques
        public int TotalBooks { get; set; } = 125; // Valeurs par défaut pour démonstration
        public int ActiveMembers { get; set; } = 43;
        public int ActiveLoans { get; set; } = 28;
        public int OverdueLoans { get; set; } = 5;
        

        // Activités récentes
        public ObservableCollection<ActivityItem> RecentActivities { get; set; }

        // ViewModels pour les différentes vues
        public BooksViewModel BooksViewModel { get; private set; }
        public AdherentsViewModel AdherentsViewModel { get; private set; } // Nouveau ViewModel pour les adhérents
        public LoansViewModel LoansViewModel { get; private set; }

        public Bibliothecaire CurrentUser { get; private set; }

        public string CurrentViewTitle
        {
            get { return _currentViewTitle; }
            set
            {
                _currentViewTitle = value;
                OnPropertyChanged(nameof(CurrentViewTitle));
            }
        }

        public DateTime CurrentDateTime
        {
            get { return _currentDateTime; }
            set
            {
                _currentDateTime = value;
                OnPropertyChanged(nameof(CurrentDateTime));
            }
        }

        public bool IsDashboardVisible
        {
            get { return _isDashboardVisible; }
            set
            {
                _isDashboardVisible = value;
                OnPropertyChanged(nameof(IsDashboardVisible));
            }
        }

        public bool IsBooksVisible
        {
            get { return _isBooksVisible; }
            set
            {
                _isBooksVisible = value;
                OnPropertyChanged(nameof(IsBooksVisible));
            }
        }

        public bool IsMembersVisible
        {
            get { return _isMembersVisible; }
            set
            {
                _isMembersVisible = value;
                OnPropertyChanged(nameof(IsMembersVisible));
            }
        }

        public ICommand NavigateCommand { get; }
        public ICommand LogoutCommand { get; }

        public MainViewModel(Bibliothecaire utilisateurConnecte, Window window)
        {
            _window = window;
            CurrentUser = utilisateurConnecte;
            CurrentViewTitle = "Tableau de bord";
            CurrentDateTime = DateTime.Now;
            
            // Par défaut, on affiche le tableau de bord avec les statistiques et on cache les autres vues
            IsDashboardVisible = true;
            IsBooksVisible = false;
            IsMembersVisible = false;
            IsLoansVisible = false;

            // Initialiser les ViewModels
            BooksViewModel = new BooksViewModel();
            AdherentsViewModel = new AdherentsViewModel(); // Initialiser le ViewModel des adhérents
            LoansViewModel = new LoansViewModel();
            
            // Initialiser les activités récentes
            InitializeRecentActivities();
    
            // Initialiser la minuterie pour mettre à jour l'heure
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(30) // Mettre à jour toutes les 30 secondes
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();

            // Commandes
            NavigateCommand = new RelayCommand(NavigateTo);
            LogoutCommand = new RelayCommand(Logout);
    
            // Charger les statistiques
            LoadStatistics();
        }

        public bool IsLoansVisible
        {
            get { return _isLoansVisible; }
            set
            {
                _isLoansVisible = value;
                OnPropertyChanged(nameof(IsLoansVisible));
            }
        }

        private void InitializeRecentActivities()
        {
            // Ces données sont pour la démonstration, normalement vous les récupéreriez de la base de données
            RecentActivities = new ObservableCollection<ActivityItem>
            {
                new ActivityItem 
                { 
                    Description = "Jean Dupont a emprunté 'Le Petit Prince'",
                    Date = DateTime.Now.AddHours(-2),
                    StatusColor = "#2ecc71" // Vert pour les actions positives
                },
                new ActivityItem 
                { 
                    Description = "Marie Martin a rendu 'Harry Potter et la chambre des secrets'",
                    Date = DateTime.Now.AddHours(-5),
                    StatusColor = "#3498db" // Bleu pour les retours
                },
                new ActivityItem 
                { 
                    Description = "Nouveau livre ajouté: '1984' de George Orwell",
                    Date = DateTime.Now.AddDays(-1),
                    StatusColor = "#9b59b6" // Violet pour les ajouts
                },
                new ActivityItem 
                { 
                    Description = "Retard de livraison: 'Les Misérables' de Victor Hugo",
                    Date = DateTime.Now.AddDays(-2),
                    StatusColor = "#e74c3c" // Rouge pour les retards
                },
                new ActivityItem 
                { 
                    Description = "Nouvel adhérent: Sophie Leclerc",
                    Date = DateTime.Now.AddDays(-3),
                    StatusColor = "#f39c12" // Orange pour les nouveaux adhérents
                }
            };
        }

        private void LoadStatistics()
        {
            // Cette méthode devrait charger les statistiques depuis la base de données
            // Pour l'instant, nous utilisons les valeurs par défaut définies dans les propriétés
            
            // Si vous avez besoin de charger depuis la BD:
            // using (var context = new BiblioGestContext())
            // {
            //     TotalBooks = context.Livre.Count();
            //     ActiveMembers = context.Adherent.Count(a => a.Actif);
            //     ActiveLoans = context.Emprunt.Count(e => e.DateRetour == null);
            //     OverdueLoans = context.Emprunt.Count(e => e.DateRetour == null && e.DateRetourPrevue < DateTime.Now);
            // }
            
            // Notifier que les propriétés ont changé
            OnPropertyChanged(nameof(TotalBooks));
            OnPropertyChanged(nameof(ActiveMembers));
            OnPropertyChanged(nameof(ActiveLoans));
            OnPropertyChanged(nameof(OverdueLoans));
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            CurrentDateTime = DateTime.Now;
        }

        private void NavigateTo(object parameter)
        {
            string destination = parameter as string;
    
            // Réinitialiser la visibilité de toutes les vues
            IsDashboardVisible = false;
            IsBooksVisible = false;
            IsMembersVisible = false;
            IsLoansVisible = false;
            // Réinitialiser les autres vues quand vous les ajouterez...

            // Afficher la vue demandée et mettre à jour le titre
            switch (destination)
            {
                case "Dashboard":
                    IsDashboardVisible = true;
                    CurrentViewTitle = "Tableau de bord";
                    break;
                case "Books":
                    IsBooksVisible = true;
                    CurrentViewTitle = "Gestion des livres";
            
                    // Rafraîchir les données des livres si nécessaire
                    BooksViewModel.Search(null);
                    break;
                case "Members":
                    IsMembersVisible = true;
                    CurrentViewTitle = "Gestion des adhérents";
            
                    // Rafraîchir les données des adhérents si nécessaire
                    AdherentsViewModel.Search(null);
                    break;
                case "Loans":
                    
                    IsLoansVisible = true;
                    CurrentViewTitle = "Gestion des emprunts";
                    LoansViewModel.Search(null);
                    break;
                case "Statistics":
                    CurrentViewTitle = "Statistiques";
                    MessageBox.Show("Les statistiques détaillées ne sont pas encore implémentées.", "Fonctionnalité à venir", MessageBoxButton.OK, MessageBoxImage.Information);
                    // Retour au tableau de bord pour l'instant
                    IsDashboardVisible = true;
                    CurrentViewTitle = "Tableau de bord";
                    break;
                case "Settings":
                    CurrentViewTitle = "Paramètres";
                    MessageBox.Show("Les paramètres ne sont pas encore implémentés.", "Fonctionnalité à venir", MessageBoxButton.OK, MessageBoxImage.Information);
                    // Retour au tableau de bord pour l'instant
                    IsDashboardVisible = true;
                    CurrentViewTitle = "Tableau de bord";
                    break;
                default:
                    IsDashboardVisible = true;
                    CurrentViewTitle = "Tableau de bord";
                    break;
            }
        }

        private void Logout(object parameter)
        {
            MessageBoxResult result = MessageBox.Show(
                "Êtes-vous sûr de vouloir vous déconnecter ?",
                "Confirmation de déconnexion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Arrêter la minuterie
                _timer.Stop();

                // Afficher la fenêtre de connexion
                var loginWindow = new LoginWindow();
                loginWindow.Show();

                // Fermer la fenêtre principale
                _window.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Classe pour les activités récentes
    public class ActivityItem
    {
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string StatusColor { get; set; }
    }
}