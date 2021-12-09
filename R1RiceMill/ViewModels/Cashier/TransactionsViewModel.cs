using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NittyGritty.Collections;
using NittyGritty.Commands;
using NittyGritty.ViewModels;
using R1RiceMill.Core;
using R1RiceMill.Data;

namespace R1RiceMill.ViewModels.Cashier
{
    public class TransactionsViewModel : ViewModelBase
    {
        public TransactionsViewModel()
        {
            Transactions = new DynamicCollection<Transaction>(Enumerable.Empty<Transaction>(), null, t => t.Date, false);
        }

        public DynamicCollection<Transaction> Transactions { get; }

        private string _search;

        public string Search
        {
            get { return _search; }
            set
            {
                Set(ref _search, value);
                Transactions.Filter = t => $"{t.TransactionNumber}".Contains(Search, StringComparison.OrdinalIgnoreCase) || t.InvoiceCode.Contains(Search, StringComparison.OrdinalIgnoreCase);
            }
        }

        private Transaction _transaction;

        public Transaction Transaction
        {
            get { return _transaction; }
            set { Set(ref _transaction, value); }
        }

        private AsyncRelayCommand _Load;
        public AsyncRelayCommand LoadCommand => _Load ?? (_Load = new AsyncRelayCommand(
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
                    Transactions.AddRange(transactions);
                }
            }));

        public override async void LoadState(object parameter, Dictionary<string, object> state)
        {
            await LoadCommand.TryExecute();
        }

        public override void SaveState(Dictionary<string, object> state)
        {
            Transactions.Clear();
        }
    }
}
