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
using MahApps.Metro.Controls;
using R1RiceMill.Core;
using R1RiceMill.Services;
using R1RiceMill.ViewModels.Inventory;

namespace R1RiceMill
{
    /// <summary>
    /// Interaction logic for InventoryWindow.xaml
    /// </summary>
    public partial class InventoryWindow : MetroWindow
    {
        private readonly NavigationService navigationService;

        public InventoryWindow(User  user)
        {
            InitializeComponent();
            (DataContext as InventoryViewModel).User = user;
            Loaded += OnWindowLoaded;
            Closed += OnWindowClosed;

            navigationService = new NavigationService();
            navigationService.Navigated += OnNavigated;
            hamburgerMenu.Content = navigationService.Frame;
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            navigationService.Navigate(new Uri("Views/Inventory/DashboardPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            Application.Current.MainWindow.Show();
        }

        private void OnNavigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            hamburgerMenu.SetCurrentValue(HamburgerMenu.SelectedItemProperty,
                hamburgerMenu
                    .Items
                    .OfType<HamburgerMenuItemBase>()
                    .FirstOrDefault(x => x.Tag.ToString() == e.Uri.ToString()));
            hamburgerMenu.SetCurrentValue(HamburgerMenu.SelectedOptionsItemProperty,
                hamburgerMenu
                    .OptionsItems
                    .OfType<HamburgerMenuItemBase>()
                    .FirstOrDefault(x => x.Tag.ToString() == e.Uri.ToString()));

            btnBack.SetCurrentValue(Button.VisibilityProperty, navigationService.CanGoBack ? Visibility.Visible : Visibility.Collapsed);
        }

        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            navigationService.GoBack();
        }

        private void OnMenuItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs args)
        {
            if (args.InvokedItem is HamburgerMenuItemBase menuItem)
            {
                navigationService.Navigate(new Uri(menuItem.Tag.ToString(), UriKind.RelativeOrAbsolute));
            }
        }

        private void OnLogout(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
