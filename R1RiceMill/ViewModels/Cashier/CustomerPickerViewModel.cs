using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NittyGritty;
using NittyGritty.Collections;
using NittyGritty.Commands;
using R1RiceMill.Core;
using R1RiceMill.Data;
using R1RiceMill.Services;
using R1RiceMill.Views.Cashier;

namespace R1RiceMill.ViewModels.Cashier
{
    public class CustomerPickerViewModel : ObservableObject
    {
        public CustomerPickerViewModel()
        {
            Customers = new DynamicCollection<Customer>(Enumerable.Empty<Customer>(), null, c => c.FirstName, true);
        }

        public DynamicCollection<Customer> Customers { get; }

        private string _search;

        public string Search
        {
            get { return _search ?? (_search = string.Empty); }
            set
            {
                Set(ref _search, value);
                Customers.Filter = c => c.FullName.Contains(Search, StringComparison.OrdinalIgnoreCase);
            }
        }

        private Customer _customer;

        public Customer Customer
        {
            get { return _customer; }
            set
            {
                Set(ref _customer, value);
                RaisePropertyChanged(nameof(IsValid));
            }
        }

        private AsyncRelayCommand _AddCustomer;
        public AsyncRelayCommand AddCustomerCommand => _AddCustomer ?? (_AddCustomer = new AsyncRelayCommand(
            async () =>
            {
                var viewModel = new AddCustomerViewModel();
                var result = WindowManager.Show<AddCustomerViewModel, CustomerPicker>(DialogKeys.AddCustomer, viewModel);
                if (result)
                {
                    var customer = viewModel.Customer;
                    using (var db = new DatabaseContext())
                    {
                        db.Customers.Add(customer);
                        await db.SaveChangesAsync();
                    }
                    Customers.Add(customer);
                }
            }));

        public async Task Load()
        {
            using (var db = new DatabaseContext())
            {
                var customers = await db.Customers.ToListAsync();
                Customers.AddRange(customers);
            }
        }

        public bool IsValid => Customer != null;
    }
}
