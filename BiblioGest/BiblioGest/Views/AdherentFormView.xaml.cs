using System.Windows;
using BiblioGest.Models;
using BiblioGest.ViewModels;

namespace BiblioGest.Views
{
    /// <summary>
    /// Logique d'interaction pour AdherentFormView.xaml
    /// </summary>
    public partial class AdherentFormView : Window
    {
        public AdherentFormView(Adherent adherent = null)
        {
            InitializeComponent();
            DataContext = new AdherentFormViewModel(this, adherent);
        }
    }
}