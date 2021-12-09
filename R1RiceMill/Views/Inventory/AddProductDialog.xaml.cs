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

namespace R1RiceMill.Views.Inventory
{
    /// <summary>
    /// Interaction logic for AddProductDialog.xaml
    /// </summary>
    public partial class AddProductDialog : Window
    {
        public AddProductDialog()
        {
            InitializeComponent();
        }

        private void OnAddClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
