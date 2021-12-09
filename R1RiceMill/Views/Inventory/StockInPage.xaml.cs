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
using R1RiceMill.ViewModels.Inventory;

namespace R1RiceMill.Views.Inventory
{
    /// <summary>
    /// Interaction logic for StockInPage.xaml
    /// </summary>
    public partial class StockInPage : Page
    {
        public StockInPage()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        public StockInViewModel ViewModel => DataContext as StockInViewModel;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadState(null, null);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.SaveState(null);
        }
    }
}
