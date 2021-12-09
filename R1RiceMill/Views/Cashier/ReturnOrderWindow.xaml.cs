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
    /// Interaction logic for ReturnOrderWindow.xaml
    /// </summary>
    public partial class ReturnOrderWindow : Window
    {
        public ReturnOrderWindow()
        {
            InitializeComponent();
        }

        ReturnOrderViewModel ViewModel => DataContext as ReturnOrderViewModel;

        private void OnReturnClick(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Order is null)
            {
                MessageBox.Show("Please select an order first");
                return;
            }

            if (string.IsNullOrWhiteSpace(ViewModel.Reason))
            {
                MessageBox.Show("You need to set the reason for return");
                return;
            }

            if (ViewModel.Quantity > ViewModel.Order.Quantity)
            {
                MessageBox.Show("You need to set a valid quantity");
                return;
            }

            DialogResult = true;
        }
    }
}
