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

namespace R1RiceMill.Views.Cashier
{
    /// <summary>
    /// Interaction logic for DiscountPicker.xaml
    /// </summary>
    public partial class DiscountPicker : Window
    {
        public DiscountPicker()
        {
            InitializeComponent();
        }

        private void OnPickClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
