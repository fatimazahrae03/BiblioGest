using System.Windows;
using BiblioGest.Models;
using BiblioGest.ViewModels;

namespace BiblioGest.Views
{
    public partial class BookFormWindow : Window
    {
        public BookFormWindow(Livre bookToEdit, System.Action refreshBooksList)
        {
            InitializeComponent();
            DataContext = new BookFormViewModel(this, refreshBooksList, bookToEdit);
        }
    }
}