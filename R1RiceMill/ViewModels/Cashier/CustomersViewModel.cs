using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using NittyGritty.Commands;
using NittyGritty.Extensions;
using NittyGritty.ViewModels;
using R1RiceMill.Core;
using R1RiceMill.Data;
using R1RiceMill.Services;

namespace R1RiceMill.ViewModels.Cashier
{
    public class CustomersViewModel : ViewModelBase
    {
        public CustomersViewModel()
        {
            Customers = new ObservableCollection<Customer>();
        }

        public ObservableCollection<Customer> Customers { get; }

        private Customer _customer;

        public Customer Customer
        {
            get { return _customer; }
            set
            {
                _customer?.CancelEdit();
                value?.BeginEdit();
                Set(ref _customer, value);
            }
        }

        private AsyncRelayCommand _Load;
        public AsyncRelayCommand LoadCommand => _Load ?? (_Load = new AsyncRelayCommand(
            async () =>
            {
                using (var db = new DatabaseContext())
                {
                    var customers = await db.Customers.ToListAsync();
                    Customers.AddRange(customers);
                }
            }));

        private AsyncRelayCommand _AddCustomer;
        public AsyncRelayCommand AddCustomerCommand => _AddCustomer ?? (_AddCustomer = new AsyncRelayCommand(
            async () =>
            {
                var viewModel = new AddCustomerViewModel();
                var result = WindowManager.Show<AddCustomerViewModel, CashierWindow>(DialogKeys.AddCustomer, viewModel);
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

        private AsyncRelayCommand _Save;
        public AsyncRelayCommand SaveCommand => _Save ?? (_Save = new AsyncRelayCommand(
            async () =>
            {
                if (Customer != null)
                {
                    if (string.IsNullOrWhiteSpace(Customer.FirstName))
                    {
                        MessageBox.Show("First name should not be empty");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(Customer.LastName))
                    {
                        MessageBox.Show("Last name should not be empty");
                        return;
                    }

                    Customer.EndEdit();
                    using (var db = new DatabaseContext())
                    {
                        db.Customers.Update(Customer);
                        await db.SaveChangesAsync();
                    }
                    Customer.BeginEdit();
                }
            }));

        public override async void LoadState(object parameter, Dictionary<string, object> state)
        {
            await LoadCommand.TryExecute();
        }

        public override void SaveState(Dictionary<string, object> state)
        {
            Customers.Clear();
        }
    }
}
