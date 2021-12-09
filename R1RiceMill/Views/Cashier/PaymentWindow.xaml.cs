using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for PaymentWindow.xaml
    /// </summary>
    public partial class PaymentWindow : Window
    {
        public PaymentWindow()
        {
            InitializeComponent();
        }

        PaymentViewModel ViewModel => DataContext as PaymentViewModel;

        private void OnPayClick(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsValid)
            {
                MessageBox.Show($"Tender amount should be above {ViewModel.GrandTotal.ToString("C2", new CultureInfo("en-PH"))}");
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
