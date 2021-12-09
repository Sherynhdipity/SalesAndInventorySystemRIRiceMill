using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NittyGritty;
using R1RiceMill.Core;
using R1RiceMill.Data;

namespace R1RiceMill.ViewModels.Cashier
{
    public class DiscountPickerViewModel : ObservableObject
    {
        public DiscountPickerViewModel(Customer customer, Discount discount, string idNumber)
        {
            Customer = customer;
            Discount = discount;
            IdNumber = idNumber;
        }

        public Customer Customer { get; }

        private IList<Discount> _discounts;

        public IList<Discount> Discounts
        {
            get { return _discounts; }
            set { Set(ref _discounts, value); }
        }

        private Discount _discount;

        public Discount Discount
        {
            get { return _discount; }
            set
            {
                Set(ref _discount, value);
                RaisePropertyChanged(nameof(IsValid));
                if (Discount != Discount.SeniorCitizen && Discount != Discount.PWD)
                {
                    IdNumber = string.Empty;
                }
            }
        }


        private string _idNumber;

        public string IdNumber
        {
            get { return _idNumber; }
            set
            {
                Set(ref _idNumber, value);
                RaisePropertyChanged(nameof(IsValid));
            }
        }

        public bool IsValid
        {
            get
            {
                switch (Discount)
                {
                    case Discount.SeniorCitizen:
                    case Discount.PWD:
                        return !string.IsNullOrWhiteSpace(IdNumber);

                    case Discount.None:
                    case Discount.Loyalty:
                    default:
                        return true;
                }
            }
        }

        public async Task Load()
        {
            var discounts = new List<Discount>();
            discounts.Add(Discount.None);
            using (var db = new DatabaseContext())
            {
                var txs = await db.Transactions
                    .Where(t => t.CustomerId == Customer.Id)
                    .Include(t => t.Orders)
                    .ToListAsync();
                var bought = txs.Sum(t => t.Orders.Sum(o => o.Quantity));
                if (bought > 100)
                {
                    discounts.Add(Discount.Loyalty);
                }
            }
            if (Customer.Age >= 60)
            {
                discounts.Add(Discount.SeniorCitizen);
            }
            discounts.Add(Discount.PWD);
            Discounts = discounts;
        }

    }
}
