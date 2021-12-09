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
using R1RiceMill.ViewModels.Cashier;

namespace R1RiceMill.Views.Cashier
{
    /// <summary>
    /// Interaction logic for AddOrderItemWindow.xaml
    /// </summary>
    public partial class AddOrderItemWindow : Window
    {
        public AddOrderItemWindow()
        {
            InitializeComponent();
        }

        public AddOrderItemViewModel ViewModel => DataContext as AddOrderItemViewModel;

        private void OnAddClick(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Product is null)
            {
                MessageBox.Show("You should select a product first");
                return;
            }
            if (ViewModel.Quantity <= 0)
            {
                MessageBox.Show("Quantity should be more than 0");
                return;
            }
            if (ViewModel.Quantity > ViewModel.Product.AvailableStock)
            {
                MessageBox.Show("Quantity exceed available stock");
                return;
            }
            DialogResult = true;
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
