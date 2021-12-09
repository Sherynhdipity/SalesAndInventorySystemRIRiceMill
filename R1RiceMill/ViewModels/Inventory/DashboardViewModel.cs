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

namespace R1RiceMill.ViewModels.Inventory
{
    public class DashboardViewModel : ViewModelBase
    {

        private IList<Batch> _batches;

        public IList<Batch> Batches
        {
            get { return _batches; }
            set { Set(ref _batches, value); }
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
