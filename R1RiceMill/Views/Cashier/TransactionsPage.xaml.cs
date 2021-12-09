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
using R1RiceMill.ViewModels.Cashier;

namespace R1RiceMill.Views.Cashier
{
    /// <summary>
    /// Interaction logic for TransactionsPage.xaml
    /// </summary>
    public partial class TransactionsPage : Page
    {
        public TransactionsPage()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        public TransactionsViewModel ViewModel => DataContext as TransactionsViewModel;

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
