using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R1RiceMill.Data;
using R1RiceMill.ViewModels;
using Admin = R1RiceMill.ViewModels.Admin;
using Cashier = R1RiceMill.ViewModels.Cashier;
using Inventory = R1RiceMill.ViewModels.Inventory;
using R1RiceMill.Services;
using R1RiceMill.Views.Admin;
using R1RiceMill.Views.Cashier;
using R1RiceMill.Views.Inventory;

namespace R1RiceMill
{
    public class Locator
    {
        public Locator()
        {
            WindowManager.Configure(DialogKeys.AddUser, typeof(AddUserDialog));
            WindowManager.Configure(DialogKeys.ChangeBatchPrice, typeof(ChangeBatchPriceDialog));
            WindowManager.Configure(DialogKeys.NewOrder, typeof(NewOrderWindow));
            WindowManager.Configure(DialogKeys.AddOrderItem, typeof(AddOrderItemWindow));
            WindowManager.Configure(DialogKeys.ReturnOrder, typeof(ReturnOrderWindow));
            WindowManager.Configure(DialogKeys.AddCustomer, typeof(AddCustomerDialog));
            WindowManager.Configure(DialogKeys.CustomerPicker, typeof(CustomerPicker));
            WindowManager.Configure(DialogKeys.DiscountPicker, typeof(DiscountPicker));
            WindowManager.Configure(DialogKeys.Payment, typeof(PaymentWindow));
            WindowManager.Configure(DialogKeys.VoidTransaction, typeof(VoidTransactionWindow));
            WindowManager.Configure(DialogKeys.AddProduct, typeof(AddProductDialog));
            WindowManager.Configure(DialogKeys.AddBatch, typeof(AddBatchDialog));

            Ioc.Default
                // Register View Models
                .AddSingleton<LoginViewModel>()
                .AddSingleton<AboutViewModel>()

                // Admin
                .AddSingleton<Admin.AdminViewModel>()
                .AddSingleton<Admin.DashboardViewModel>()
                .AddSingleton<Admin.UsersViewModel>()
                .AddSingleton<Admin.ProductsViewModel>()
                .AddSingleton<Admin.ReportsViewModel>()
                .AddSingleton<Admin.BackupRestoreViewModel>()

                // Cashier
                .AddSingleton<Cashier.CashierViewModel>()
                .AddSingleton<Cashier.DashboardViewModel>()
                .AddSingleton<Cashier.CustomersViewModel>()
                .AddSingleton<Cashier.TransactionsViewModel>()
                .AddSingleton<Cashier.ReportsViewModel>()

                // Inventory
                .AddSingleton<Inventory.InventoryViewModel>()
                .AddSingleton<Inventory.DashboardViewModel>()
                .AddSingleton<Inventory.StockInViewModel>()
                .AddSingleton<Inventory.ReportsViewModel>()

                .Build();
        }

        public LoginViewModel Login => Ioc.Default.GetInstance<LoginViewModel>();
        public AboutViewModel About => Ioc.Default.GetInstance<AboutViewModel>();

        // Admin
        public Admin.AdminViewModel AdminMain => Ioc.Default.GetInstance<Admin.AdminViewModel>();
        public Admin.DashboardViewModel AdminDashboard => Ioc.Default.GetInstance<Admin.DashboardViewModel>();
        public Admin.UsersViewModel AdminUsers => Ioc.Default.GetInstance<Admin.UsersViewModel>();
        public Admin.ProductsViewModel AdminProducts => Ioc.Default.GetInstance<Admin.ProductsViewModel>();
        public Admin.ReportsViewModel AdminReports => Ioc.Default.GetInstance<Admin.ReportsViewModel>();
        public Admin.BackupRestoreViewModel AdminBackupRestore => Ioc.Default.GetInstance<Admin.BackupRestoreViewModel>();

        // Cashier
        public Cashier.CashierViewModel CashierMain => Ioc.Default.GetInstance<Cashier.CashierViewModel>();
        public Cashier.DashboardViewModel CashierDashboard => Ioc.Default.GetInstance<Cashier.DashboardViewModel>();
        public Cashier.CustomersViewModel CashierCustomers => Ioc.Default.GetInstance<Cashier.CustomersViewModel>();
        public Cashier.TransactionsViewModel CashierTransactions => Ioc.Default.GetInstance<Cashier.TransactionsViewModel>();
        public Cashier.ReportsViewModel CashierReports => Ioc.Default.GetInstance<Cashier.ReportsViewModel>();

        // Inventory
        public Inventory.InventoryViewModel InventoryMain => Ioc.Default.GetInstance<Inventory.InventoryViewModel>();
        public Inventory.DashboardViewModel InventoryDashboard => Ioc.Default.GetInstance<Inventory.DashboardViewModel>();
        public Inventory.StockInViewModel InventoryStockIn => Ioc.Default.GetInstance<Inventory.StockInViewModel>();
        public Inventory.ReportsViewModel InventoryReports => Ioc.Default.GetInstance<Inventory.ReportsViewModel>();
    }

    public static class DialogKeys
    {
        public const string AddUser = nameof(AddUser);

        public const string ChangeBatchPrice = nameof(ChangeBatchPrice);

        public const string NewOrder = nameof(NewOrder);

        public const string AddOrderItem = nameof(AddOrderItem);

        public const string ReturnOrder = nameof(ReturnOrder);

        public const string AddCustomer = nameof(AddCustomer);

        public const string CustomerPicker = nameof(CustomerPicker);

        public const string DiscountPicker = nameof(DiscountPicker);

        public const string Payment = nameof(Payment);

        public const string VoidTransaction = nameof(VoidTransaction);

        public const string AddProduct = nameof(AddProduct);

        public const string AddBatch = nameof(AddBatch);

    }
}
