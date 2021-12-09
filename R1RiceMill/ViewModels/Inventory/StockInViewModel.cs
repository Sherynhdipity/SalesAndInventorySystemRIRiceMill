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
using R1RiceMill.Services;

namespace R1RiceMill.ViewModels.Inventory
{
    public class StockInViewModel : ViewModelBase
    {
        public StockInViewModel()
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

        private AsyncRelayCommand _AddProduct;
        public AsyncRelayCommand AddProductCommand => _AddProduct ?? (_AddProduct = new AsyncRelayCommand(
            async () =>
            {
                var viewModel = new AddProductViewModel();
                var result = WindowManager.Show<AddProductViewModel, InventoryWindow>(DialogKeys.AddProduct, viewModel);
                if (result)
                {
                    using (var db = new DatabaseContext())
                    {
                        var product = new Product
                        {
                            Variety = viewModel.Variety,
                            Description = viewModel.Description
                        };
                        db.Products.Add(product);
                        await db.SaveChangesAsync();
                        Products.Add(product);
                    }
                }
            }));

        private AsyncRelayCommand _StockIn;
        public AsyncRelayCommand StockInCommand => _StockIn ?? (_StockIn = new AsyncRelayCommand(
            async () =>
            {
                if (Product != null)
                {
                    var viewModel = new AddBatchViewModel();
                    var result = WindowManager.Show<AddBatchViewModel, InventoryWindow>(DialogKeys.AddBatch, viewModel);
                    if (result)
                    {
                        using (var db = new DatabaseContext())
                        {
                            var batch = new Batch
                            {
                                ProductId = Product.Id,
                                MillingDate = viewModel.MillingDate.Value,
                                Price = viewModel.Price.Value,
                                Quantity = viewModel.Quantity.Value,
                                Date = DateTime.Now
                            };
                            db.Batches.Add(batch);
                            await db.SaveChangesAsync();
                            Product.Batches.Add(batch);
                            Product.RaiseAllPropertiesChanged();
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
