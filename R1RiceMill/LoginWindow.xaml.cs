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
using R1RiceMill.ViewModels;

namespace R1RiceMill
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            IsVisibleChanged += OnIsVisibleChanged;
        }

        public LoginViewModel ViewModel => DataContext as LoginViewModel;

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                ViewModel.Username = string.Empty;
                ViewModel.Password = string.Empty;
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    txtUsername.Focus();
                }), System.Windows.Threading.DispatcherPriority.Render);
                ViewModel.LoggedInUser = null;
            }
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModel.Password = (sender as PasswordBox).Password;
        }
    }
}
