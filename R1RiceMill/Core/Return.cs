using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NittyGritty;

namespace R1RiceMill.Core
{
    public class Return : ObservableObject
    {

        private int _id;

        public int Id
        {
            get { return _id; }
            set { Set(ref _id, value); }
        }

        private int _orderId;

        public int OrderId
        {
            get { return _orderId; }
            set { Set(ref _orderId, value); }
        }

        private double _quantity;

        public double Quantity
        {
            get { return _quantity; }
            set { Set(ref _quantity, value); }
        }

        private string _reason;

        public string Reason
        {
            get { return _reason; }
            set { Set(ref _reason, value); }
        }

        private DateTime _date;

        public DateTime Date
        {
            get { return _date; }
            set { Set(ref _date, value); }
        }

        public Order Order { get; set; }

    }
}
