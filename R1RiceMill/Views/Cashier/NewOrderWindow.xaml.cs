using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using R1RiceMill.Services;
using R1RiceMill.ViewModels.Cashier;

namespace R1RiceMill.Views.Cashier
{
    /// <summary>
    /// Interaction logic for NewOrderWindow.xaml
    /// </summary>
    public partial class NewOrderWindow : Window
    {
        public NewOrderWindow()
        {
            InitializeComponent();
            Closing += OnWindowClosing;
        }

        public NewOrderViewModel ViewModel => DataContext as NewOrderViewModel;

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (ViewModel.TransactionStatus == TransactionStatus.None)
            {
                MessageBox.Show("Transaction needs to be void or paid first before closing.", "Transaction Not Ready", MessageBoxButton.OK, MessageBoxImage.Warning);
                e.Cancel = true;
            }
        }

        private void OnPayClick(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Customer is null)
            {
                MessageBox.Show("You should set the Customer first");
                return;
            }
            if (ViewModel.Orders.Count == 0)
            {
                MessageBox.Show("Add an order to the list first");
                return;
            }

            var viewModel = new PaymentViewModel(ViewModel.Total);
            var result = WindowManager.Show<PaymentViewModel, NewOrderWindow>(DialogKeys.Payment, viewModel);
            if (result)
            {
                ViewModel.Payment = viewModel.Payment.GetValueOrDefault();
                ViewModel.TransactionStatus = TransactionStatus.Paid;
                DialogResult = true;
            }
        }

        private void OnVoidClick(object sender, RoutedEventArgs e)
        {
            var viewModel = new VoidTransactionViewModel();
            var result = WindowManager.Show<VoidTransactionViewModel, NewOrderWindow>(DialogKeys.VoidTransaction, viewModel);
            if (result)
            {
                ViewModel.TransactionStatus = TransactionStatus.Void;
                DialogResult = false;
            }
        }

    }
}
