using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NittyGritty;
using R1RiceMill.Core;

namespace R1RiceMill.ViewModels.Cashier
{
    public class AddCustomerViewModel : ObservableObject
    {
        public AddCustomerViewModel()
        {
            Customer = new Customer();
            Customer.PropertyChanged += OnCustomerPropertyChanged;
        }

        private void OnCustomerPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(IsValid));
        }

        public Customer Customer { get; }

        public bool IsValid => !string.IsNullOrWhiteSpace(Customer.FirstName) &&
            !string.IsNullOrWhiteSpace(Customer.LastName);
    }
}
