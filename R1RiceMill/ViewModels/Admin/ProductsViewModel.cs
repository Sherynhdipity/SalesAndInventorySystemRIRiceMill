using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NittyGritty;
using NittyGritty.Collections;
using NittyGritty.Commands;
using NittyGritty.ViewModels;
using R1RiceMill.Core;
using R1RiceMill.Data;
using R1RiceMill.Services;

namespace R1RiceMill.ViewModels.Admin
{
    public class ProductsViewModel : ViewModelBase
    {
        public ProductsViewModel()
        {
            Products = new DynamicCollection<Product>(Enumerable.Empty<Product>(), null, p => p.Variety, true);
        }

        public DynamicCollection<Product> Products { get; }

        private Product _product;

        public Product Product
        {
            get { return _product; }
            set { Set(ref _product, value); }
        }

        private AsyncRelayCommand<Batch> _ChangePrice;
        public AsyncRelayCommand<Batch> ChangePriceCommand => _ChangePrice ?? (_ChangePrice = new AsyncRelayCommand<Batch>(
            async (batch) =>
            {
                if (batch != null)
                {
                    var viewModel = new ChangeBatchPriceViewModel(batch, batch.Price);
                    var result = WindowManager.Show<ChangeBatchPriceViewModel, AdminWindow>(DialogKeys.ChangeBatchPrice, viewModel);
                    if (result)
                    {
                        batch.Price = viewModel.NewPrice.Value;
                        using (var db = new DatabaseContext())
                        {
                            db.Batches.Update(batch);
                            await db.SaveChangesAsync();
                        }
                    }
                }
            }));

        private AsyncRelayCommand _Load;
        public AsyncRelayCommand LoadCommand => _Load ?? (_Load = new AsyncRelayCommand(
            async () =>
            {
                using (var db = new DatabaseContext())
                {
                    var products = await db.Products
                        .Include(p => p.Batches)
                        .ThenInclude(b => b.Orders)
                        .ToListAsync();
                    Products.AddRange(products);
                }
            }));

        public override async void LoadState(object parameter, Dictionary<string, object> state)
        {
            await LoadCommand.TryExecute();
        }

        public override void SaveState(Dictionary<string, object> state)
        {
            Products.Clear();
        }
    }
}
