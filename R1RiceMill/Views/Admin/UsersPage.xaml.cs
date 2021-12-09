using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using R1RiceMill.ViewModels.Admin;

namespace R1RiceMill.Views.Admin
{
    /// <summary>
    /// Interaction logic for UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        public UsersPage()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        public UsersViewModel ViewModel => DataContext as UsersViewModel;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadState(null, null);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.SaveState(null);
        }

        private void OnNewPasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModel.NewPassword = (sender as PasswordBox).Password;
        }

        private void OnConfirmPasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModel.ConfirmPassword = (sender as PasswordBox).Password;
        }
    }
}
