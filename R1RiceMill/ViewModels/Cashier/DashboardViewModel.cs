using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NittyGritty.Commands;
using NittyGritty.ViewModels;
using R1RiceMill.Core;
using R1RiceMill.Data;
using R1RiceMill.Services;

namespace R1RiceMill.ViewModels.Cashier
{
    public class DashboardViewModel : ViewModelBase
    {

        private AsyncRelayCommand _NewOrder;
        public AsyncRelayCommand NewOrderCommand => _NewOrder ?? (_NewOrder = new AsyncRelayCommand(
            async () =>
            {
                var viewModel = new NewOrderViewModel();
                var result = WindowManager.Show<NewOrderViewModel, CashierWindow>(DialogKeys.NewOrder, viewModel);
                if (result)
                {
                    var now = DateTime.Now;
                    var txNumber = Transaction.GenerateTransactionNumber(now);
                    var transaction = new Transaction()
                    {
                        Date = now,
                        TransactionNumber = txNumber,
                        InvoiceCode = Transaction.GenerateInvoiceCode(txNumber),
                        UserId = Ioc.Default.GetInstance<CashierViewModel>().User.Id,
                        CustomerId = viewModel.Customer.Id,
                        Discount = viewModel.DiscountType,
                        DiscountIdNumber = viewModel.DiscountIdNumber,
                        Payment = viewModel.Payment
                    };

                    var batch = viewModel.Orders.FirstOrDefault()?.Batch;
                    using (var db = new DatabaseContext())
                    {
                        db.Transactions.Add(transaction);
                        await db.SaveChangesAsync();
                        foreach (var item in viewModel.Orders)
                        {
                            item.Batch = null;
                            item.TransactionId = transaction.Id;
                        }
                        db.Orders.AddRange(viewModel.Orders);
                        await db.SaveChangesAsync();
                    }

                    foreach (var item in viewModel.Orders)
                    {
                        item.Batch = batch;
                    }
                    transaction.Customer = viewModel.Customer;
                    transaction.User = Ioc.Default.GetInstance<CashierViewModel>().User;
                    transaction.Orders = viewModel.Orders;
                    ReportService.Receipt(transaction, viewModel.SubTotal, viewModel.DiscountType, viewModel.Discount, viewModel.Tax, viewModel.Total);
                }
            }));

        private AsyncRelayCommand _ReturnOrder;
        public AsyncRelayCommand ReturnOrderCommand => _ReturnOrder ?? (_ReturnOrder = new AsyncRelayCommand(
            async () =>
            {
                var viewModel = new ReturnOrderViewModel();
                var result = WindowManager.Show<ReturnOrderViewModel, CashierWindow>(DialogKeys.ReturnOrder, viewModel);
                if (result)
                {
                    var returnOrder = new Return
                    {
                        Date = DateTime.Now,
                        Order = viewModel.Order,
                        OrderId = viewModel.Order.Id,
                        Quantity = viewModel.Quantity.Value,
                        Reason = viewModel.Reason,
                    };
                    using (var db = new DatabaseContext())
                    {
                        db.Returns.Add(returnOrder);
                        await db.SaveChangesAsync();
                    }
                }
            }));

        private IList<Order> _orders;

        public IList<Order> Orders
        {
            get { return _orders; }
            set { Set(ref _orders, value); }
        }

        private IList<Return> _returns;

        public IList<Return> Returns
        {
            get { return _returns; }
            set { Set(ref _returns, value); }
        }

        private AsyncRelayCommand _Load;
        public AsyncRelayCommand LoadCommand => _Load ?? (_Load = new AsyncRelayCommand(
            async () =>
            {
                var now = DateTime.Now.Date.AddDays(1);
                var past30 = now.AddDays(-30);
                using (var db = new DatabaseContext())
                {
                    var orders = await db.Orders.Include(o => o.Transaction).ToListAsync();
                    Orders = orders.Where(o => o.Transaction.Date < now && o.Transaction.Date >= past30).ToList();

                    var returns = await db.Returns.ToListAsync();
                    Returns = returns.Where(r => r.Date < now && r.Date >= past30).ToList();
                };
            }));

        public override async void LoadState(object parameter, Dictionary<string, object> state)
        {
            await LoadCommand.TryExecute();
        }

        public override void SaveState(Dictionary<string, object> state)
        {
            
        }
    }
}
