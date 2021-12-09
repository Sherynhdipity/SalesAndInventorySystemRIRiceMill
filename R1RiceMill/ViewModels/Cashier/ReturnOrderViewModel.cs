using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NittyGritty;
using NittyGritty.Commands;
using R1RiceMill.Core;
using R1RiceMill.Data;

namespace R1RiceMill.ViewModels.Cashier
{
    public class ReturnOrderViewModel : ObservableObject
    {

        private string _search;

        public string Search
        {
            get { return _search; }
            set { Set(ref _search, value); }
        }

        private Transaction _transaction;

        public Transaction Transaction
        {
            get { return _transaction; }
            set { Set(ref _transaction, value); }
        }

        private Order _order;

        public Order Order
        {
            get { return _order; }
            set { Set(ref _order, value); }
        }

        private string _reason;

        public string Reason
        {
            get { return _reason; }
            set { Set(ref _reason, value); }
        }

        private double? _quantity;

        public double? Quantity
        {
            get { return _quantity; }
            set { Set(ref _quantity, value); }
        }

        private AsyncRelayCommand _Search;
        public AsyncRelayCommand SearchCommand => _Search ?? (_Search = new AsyncRelayCommand(
            async () =>
            {
                using (var db = new DatabaseContext())
                {
                    var transactions = await db.Transactions
                        .Include(t => t.Customer)
                        .Include(t => t.Orders)
                        .ThenInclude(o => o.Batch)
                        .ThenInclude(b => b.Product)
                        .ToListAsync();
                    Transaction = transactions.FirstOrDefault(t => t.TransactionNumber.ToString() == Search || t.InvoiceCode.Equals(Search, StringComparison.OrdinalIgnoreCase));
                }
            }));

    }
}
