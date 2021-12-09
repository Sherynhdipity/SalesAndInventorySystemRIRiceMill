using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NittyGritty;
using NittyGritty.Commands;
using NittyGritty.Extensions;
using NittyGritty.ViewModels;
using R1RiceMill.Core;
using R1RiceMill.Data;

namespace R1RiceMill.ViewModels.Admin
{
    public class DashboardViewModel : ViewModelBase
    {
        public DashboardViewModel()
        {
            
        }

        private IList<Batch> _batches;

        public IList<Batch> Batches
        {
            get { return _batches; }
            set { Set(ref _batches, value); }
        }

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

        private IList<Batch> _lowStock;

        public IList<Batch> LowStock
        {
            get { return _lowStock; }
            set { Set(ref _lowStock, value); }
        }

        private AsyncRelayCommand _Load;
        public AsyncRelayCommand LoadCommand => _Load ?? (_Load = new AsyncRelayCommand(
            async () =>
            {
                var now = DateTime.Now.Date.AddDays(1);
                var past30 = now.AddDays(-30);
                using (var db = new DatabaseContext())
                {
                    var batches = await db.Batches.Include(b => b.Product).ToListAsync();
                    Batches = batches.Where(b => b.Date < now && b.Date >= past30).ToList();
                    LowStock = Batches.Where(b => b.AvailableStock <= 100 && b.AvailableStock > 0).ToList();

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
