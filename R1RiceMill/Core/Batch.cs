using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NittyGritty;

namespace R1RiceMill.Core
{
    public class Batch : ObservableObject
    {

        private int _id;

        public int Id
        {
            get { return _id; }
            set { Set(ref _id, value); }
        }

        private int _productId;

        public int ProductId
        {
            get { return _productId; }
            set { Set(ref _productId, value); }
        }

        private DateTime _millingDate;

        public DateTime MillingDate
        {
            get { return _millingDate; }
            set { Set(ref _millingDate, value); }
        }

        private decimal _price;

        public decimal Price
        {
            get { return _price; }
            set { Set(ref _price, value); }
        }

        private double _quantity;

        public double Quantity
        {
            get { return _quantity; }
            set { Set(ref _quantity, value); }
        }

        private DateTime _date;

        public DateTime Date
        {
            get { return _date; }
            set { Set(ref _date, value); }
        }

        public Product Product { get; set; }
        
        private ObservableCollection<Order> orders;
        public ObservableCollection<Order> Orders { get => orders ?? (orders = new ObservableCollection<Order>()); set => orders = value; }

        [NotMapped]
        public string Code => $"{Date:MM-dd-yyyy HH:mm:ss}";

        [NotMapped]
        public double AvailableStock => Quantity - Orders.Sum(o => o.Quantity);
    }
}
