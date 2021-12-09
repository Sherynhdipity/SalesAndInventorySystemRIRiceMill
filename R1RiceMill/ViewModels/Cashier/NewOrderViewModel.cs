using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlzEx.Standard;
using NittyGritty;
using NittyGritty.Commands;
using NittyGritty.Extensions;
using R1RiceMill.Core;
using R1RiceMill.Services;
using R1RiceMill.Views.Cashier;

namespace R1RiceMill.ViewModels.Cashier
{
    public class NewOrderViewModel : ObservableObject
    {
        public NewOrderViewModel()
        {
            Orders = new ObservableCollection<Order>();
            Orders.CollectionChanged += OnOrdersCollectionChanged;
        }

        private void OnOrdersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Recompute();
        }

        private void Recompute()
        {
            RaisePropertyChanged(nameof(SubTotal));
            RaisePropertyChanged(nameof(Discount));
            RaisePropertyChanged(nameof(Tax));
            RaisePropertyChanged(nameof(Total));
        }

        private Customer _customer;

        public Customer Customer
        {
            get { return _customer; }
            set { Set(ref _customer, value); }
        }

        public ObservableCollection<Order> Orders { get; }

        public decimal SubTotal => Orders.Sum(o => (decimal)o.Quantity * o.Batch.Price);

        public decimal Discount => DiscountType.ComputeDiscount(SubTotal);

        public decimal Tax => SubTotal * 0.12M;

        public decimal Total => SubTotal + Tax - Discount;

        private string _discountIdNumber;

        public string DiscountIdNumber
        {
            get { return _discountIdNumber; }
            set { Set(ref _discountIdNumber, value); }
        }

        private TransactionStatus _transactionStatus;

        public TransactionStatus TransactionStatus
        {
            get { return _transactionStatus; }
            set { Set(ref _transactionStatus, value); }
        }

        private Discount _discountType;

        public Discount DiscountType
        {
            get { return _discountType; }
            set
            {
                Set(ref _discountType, value);
                Recompute();
            }
        }

        private decimal _payment;

        public decimal Payment
        {
            get { return _payment; }
            set { Set(ref _payment, value); }
        }

        private RelayCommand _PickCustomer;
        public RelayCommand PickCustomerCommand => _PickCustomer ?? (_PickCustomer = new RelayCommand(
            () =>
            {
                var viewModel = new CustomerPickerViewModel();
                var result = WindowManager.Show<CustomerPickerViewModel, NewOrderWindow>(DialogKeys.CustomerPicker, viewModel, (vm) => viewModel.Load());
                if (result)
                {
                    Customer = viewModel.Customer;
                }
            }));

        private RelayCommand _AddOrderItem;
        public RelayCommand AddOrderItemCommand => _AddOrderItem ?? (_AddOrderItem = new RelayCommand(
            () =>
            {
                var viewModel = new AddOrderItemViewModel();
                var result = WindowManager.Show<AddOrderItemViewModel, NewOrderWindow>(DialogKeys.AddOrderItem, viewModel, (vm) => vm.Load());
                if (result)
                {
                    Orders.AddRange(viewModel.GetOrders());
                }
            }));

        private RelayCommand _PickDiscount;
        public RelayCommand PickDiscountCommand => _PickDiscount ?? (_PickDiscount = new RelayCommand(
            () =>
            {
                var viewModel = new DiscountPickerViewModel(Customer, DiscountType, DiscountIdNumber);
                var result = WindowManager.Show<DiscountPickerViewModel, NewOrderWindow>(DialogKeys.DiscountPicker, viewModel, (vm) => vm.Load());
                if (result)
                {
                    DiscountType = viewModel.Discount;
                    DiscountIdNumber = viewModel.IdNumber;
                }
            }));

    }

    public enum TransactionStatus
    {
        None = 0,
        Void = 1,
        Paid = 2
    }
}
