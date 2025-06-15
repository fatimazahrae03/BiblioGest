using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using BiblioGest.Data;
using BiblioGest.Models;
using BiblioGest.Services;
using Microsoft.EntityFrameworkCore;

namespace BiblioGest.ViewModels
{
    public class BookFormViewModel : INotifyPropertyChanged
    {
        private readonly BiblioGestContext _dbContext;
        private readonly Window _dialogWindow;
        private readonly Action _refreshBooksList;
        private Livre _currentBook;
        private Categorie _selectedCategory;
        private string _errorMessage;
        private bool _isEditMode;
        private Visibility _imagePreviewVisibility = Visibility.Collapsed;

        public BookFormViewModel(Window dialogWindow, Action refreshBooksList, Livre bookToEdit = null)
        {
            _dbContext = new BiblioGestContext();
            _dialogWindow = dialogWindow;
            _refreshBooksList = refreshBooksList;
            
            // Détermine si nous sommes en mode édition ou ajout
            _isEditMode = bookToEdit != null;
            
            // Initialise le livre courant (copie ou nouveau)
            if (_isEditMode)
            {
                // Créer une copie du livre pour l'édition
                CurrentBook = new Livre
                {
                    LivreId = bookToEdit.LivreId,
                    Titre = bookToEdit.Titre,
                    Auteur = bookToEdit.Auteur,
                    ISBN = bookToEdit.ISBN,
                    Annee = bookToEdit.Annee,
                    Editeur = bookToEdit.Editeur,
                    Resume = bookToEdit.Resume,
                    NombreExemplaires = bookToEdit.NombreExemplaires,
                    CategorieId = bookToEdit.CategorieId,
                    ImageCouverture = bookToEdit.ImageCouverture
                };
                
                // Afficher la prévisualisation de l'image si disponible
                if (!string.IsNullOrWhiteSpace(CurrentBook.ImageCouverture))
                {
                    ImagePreviewVisibility = Visibility.Visible;
                }
            }
            else
            {
                // Créer un nouveau livre vide
                CurrentBook = new Livre
                {
                    NombreExemplaires = 1, // Valeur par défaut
                    Annee = DateTime.Now.Year // Année courante par défaut
                };
            }

            // Charger les catégories
            LoadCategories();
            
            // Si en mode édition, sélectionner la catégorie du livre
            if (_isEditMode && bookToEdit.Categorie != null)
            {
                SelectedCategory = Categories.FirstOrDefault(c => c.CategorieId == bookToEdit.CategorieId);
            }
            else if (Categories.Count > 0)
            {
                // Par défaut, sélectionner la première catégorie
                SelectedCategory = Categories[0];
            }

            // Initialiser les commandes
            SaveCommand = new RelayCommand(SaveBook, CanSaveBook);
            CancelCommand = new RelayCommand(Cancel);
            PreviewImageCommand = new RelayCommand(PreviewImage, CanPreviewImage);
        }

        private void LoadCategories()
        {
            try
            {
                // Charger toutes les catégories
                var categories = _dbContext.Categorie.ToList();
                Categories = new ObservableCollection<Categorie>(categories);
                
                if (Categories.Count == 0)
                {
                    // Ajouter une catégorie par défaut si aucune n'existe
                    ErrorMessage = "Attention: Aucune catégorie n'est disponible. Veuillez d'abord créer des catégories.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des catégories: {ex.Message}";
                Categories = new ObservableCollection<Categorie>();
            }
        }

        public Livre CurrentBook
        {
            get { return _currentBook; }
            set
            {
                _currentBook = value;
                OnPropertyChanged(nameof(CurrentBook));
            }
        }

        public ObservableCollection<Categorie> Categories { get; private set; }

        public Categorie SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                if (_selectedCategory != null)
                {
                    CurrentBook.CategorieId = _selectedCategory.CategorieId;
                    CurrentBook.Categorie = _selectedCategory;
                }
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public Visibility ImagePreviewVisibility
        {
            get { return _imagePreviewVisibility; }
            set
            {
                _imagePreviewVisibility = value;
                OnPropertyChanged(nameof(ImagePreviewVisibility));
            }
        }

        public string FormTitle => _isEditMode ? "Modifier un livre" : "Ajouter un nouveau livre";
        
        public string SaveButtonText => _isEditMode ? "Enregistrer" : "Ajouter";

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand PreviewImageCommand { get; }

        private bool CanPreviewImage(object parameter)
        {
            return !string.IsNullOrWhiteSpace(CurrentBook?.ImageCouverture);
        }

        private void PreviewImage(object parameter)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(CurrentBook.ImageCouverture))
                {
                    // Vérifier si l'URL est valide
                    Uri imageUri;
                    bool isValid = Uri.TryCreate(CurrentBook.ImageCouverture, UriKind.Absolute, out imageUri) &&
                                 (imageUri.Scheme == Uri.UriSchemeHttp || imageUri.Scheme == Uri.UriSchemeHttps);
                    
                    if (isValid)
                    {
                        // Tenter de charger l'image pour valider
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = imageUri;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        
                        // Afficher la prévisualisation
                        ImagePreviewVisibility = Visibility.Visible;
                    }
                    else
                    {
                        ErrorMessage = "L'URL de l'image n'est pas valide.";
                    }
                }
                else
                {
                    ImagePreviewVisibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement de l'image: {ex.Message}";
                ImagePreviewVisibility = Visibility.Collapsed;
            }
        }

        private bool CanSaveBook(object parameter)
        {
            // Vérification basique des champs obligatoires
            return !string.IsNullOrWhiteSpace(CurrentBook.Titre) &&
                   !string.IsNullOrWhiteSpace(CurrentBook.Auteur) &&
                   CurrentBook.NombreExemplaires > 0 &&
                   SelectedCategory != null;
        }

        private void SaveBook(object parameter)
        {
            try
            {
                // Vérifier les champs obligatoires
                if (string.IsNullOrWhiteSpace(CurrentBook.Titre))
                {
                    ErrorMessage = "Le titre est obligatoire.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(CurrentBook.Auteur))
                {
                    ErrorMessage = "L'auteur est obligatoire.";
                    return;
                }

                if (CurrentBook.NombreExemplaires <= 0)
                {
                    ErrorMessage = "Le nombre d'exemplaires doit être supérieur à 0.";
                    return;
                }

                if (SelectedCategory == null)
                {
                    ErrorMessage = "Veuillez sélectionner une catégorie.";
                    return;
                }

                // Vérifier que l'URL de l'image est valide, si elle est fournie
                if (!string.IsNullOrWhiteSpace(CurrentBook.ImageCouverture))
                {
                    Uri imageUri;
                    bool isValid = Uri.TryCreate(CurrentBook.ImageCouverture, UriKind.Absolute, out imageUri) &&
                                 (imageUri.Scheme == Uri.UriSchemeHttp || imageUri.Scheme == Uri.UriSchemeHttps);
                    
                    if (!isValid)
                    {
                        ErrorMessage = "L'URL de l'image n'est pas valide.";
                        return;
                    }
                }

                // Assigner la catégorie sélectionnée
                CurrentBook.CategorieId = SelectedCategory.CategorieId;

                if (_isEditMode)
                {
                    // Mettre à jour le livre existant
                    var bookToUpdate = _dbContext.Livre.Find(CurrentBook.LivreId);
                    if (bookToUpdate != null)
                    {
                        // Mettre à jour les propriétés
                        bookToUpdate.Titre = CurrentBook.Titre;
                        bookToUpdate.Auteur = CurrentBook.Auteur;
                        bookToUpdate.ISBN = CurrentBook.ISBN;
                        bookToUpdate.Annee = CurrentBook.Annee;
                        bookToUpdate.Editeur = CurrentBook.Editeur;
                        bookToUpdate.Resume = CurrentBook.Resume;
                        bookToUpdate.NombreExemplaires = CurrentBook.NombreExemplaires;
                        bookToUpdate.CategorieId = CurrentBook.CategorieId;
                        bookToUpdate.ImageCouverture = CurrentBook.ImageCouverture;
                        
                        _dbContext.SaveChanges();
                        MessageBox.Show("Le livre a été mis à jour avec succès.", "Succès", 
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    // Ajouter un nouveau livre
                    _dbContext.Livre.Add(CurrentBook);
                    _dbContext.SaveChanges();
                    MessageBox.Show("Le livre a été ajouté avec succès.", "Succès", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }

                // Rafraîchir la liste des livres
                _refreshBooksList?.Invoke();
                
                // Fermer la fenêtre de dialogue
                _dialogWindow.DialogResult = true;
                _dialogWindow.Close();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de l'enregistrement: {ex.Message}";
            }
        }

        private void Cancel(object parameter)
        {
            // Fermer la fenêtre sans sauvegarder
            _dialogWindow.DialogResult = false;
            _dialogWindow.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}