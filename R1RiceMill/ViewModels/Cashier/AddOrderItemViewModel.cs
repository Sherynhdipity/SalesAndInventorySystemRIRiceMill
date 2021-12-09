using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NittyGritty;
using NittyGritty.Collections;
using R1RiceMill.Core;
using R1RiceMill.Data;

namespace R1RiceMill.ViewModels.Cashier
{
    public class AddOrderItemViewModel : ObservableObject
    {
        public AddOrderItemViewModel()
        {
            Products = new DynamicCollection<Product>(Enumerable.Empty<Product>(), p => p.AvailableStock > 0, p => p.Variety, true);
        }

        public DynamicCollection<Product> Products { get; }

        private Product _product;

        public Product Product
        {
            get { return _product; }
            set
            {
                Set(ref _product, value);
                RaisePropertyChanged(nameof(IsValid));
            }
        }

        private double? _quantity;

        public double? Quantity
        {
            get { return _quantity; }
            set
            {
                Set(ref _quantity, value);
                RaisePropertyChanged(nameof(IsValid));
            }
        }
        
        public IList<Order> GetOrders()
        {
            var orders = new List<Order>();
            if (IsValid)
            {
                var quantity = Quantity.Value;
                var batches = Product.Batches.Where(b => b.AvailableStock > 0).OrderBy(b => b.Date);
                foreach (var item in batches)
                {
                    item.Product = Product;
                    if (item.AvailableStock >= quantity)
                    {
                        orders.Add(new Order
                        {
                            BatchId = item.Id,
                            Batch = item,
                            Quantity = quantity,
                            Price = item.Price
                        });
                        break;
                    }
                    else
                    {
                        orders.Add(new Order
                        {
                            BatchId = item.Id,
                            Batch = item,
                            Quantity = item.AvailableStock,
                            Price = item.Price
                        });
                        quantity = quantity - item.AvailableStock;
                    }
                }
            }
            return orders;
        }

        public bool IsValid => Product != null && Quantity > 0 & Product.AvailableStock >= Quantity;

        public async Task Load()
        {
            using (var db = new DatabaseContext())
            {
                var products = await db.Products
                    .Include(p => p.Batches)
                    .ThenInclude(b => b.Orders)
                    .ToListAsync();
                Products.AddRange(products);
            }
        }
    }
}
