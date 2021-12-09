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
    /// Interaction logic for VoidTransactionWindow.xaml
    /// </summary>
    public partial class VoidTransactionWindow : Window
    {
        public VoidTransactionWindow()
        {
            InitializeComponent();
        }

        private async void OnVoidClick(object sender, RoutedEventArgs e)
        {
            var successful = await ((VoidTransactionViewModel)DataContext).AdminLogin();
            if (successful)
            {
                DialogResult = true;
            }
        }
    }
}
