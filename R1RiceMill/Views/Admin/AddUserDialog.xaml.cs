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
using System.Windows.Shapes;
using R1RiceMill.ViewModels.Admin;

namespace R1RiceMill.Views.Admin
{
    /// <summary>
    /// Interaction logic for AddUserDialog.xaml
    /// </summary>
    public partial class AddUserDialog : Window
    {
        public AddUserDialog()
        {
            InitializeComponent();
        }

        public AddUserViewModel ViewModel => DataContext as AddUserViewModel;

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModel.Password = (sender as PasswordBox).Password;
        }

        private void OnConfirmPasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModel.ConfirmPassword = (sender as PasswordBox).Password;
        }
    }
}
