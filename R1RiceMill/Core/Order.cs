using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NittyGritty;

namespace R1RiceMill.Core
{
    public class Order : ObservableObject
    {

        private int _id;

        public int Id
        {
            get { return _id; }
            set { Set(ref _id, value); }
        }

        private int _transactionId;

        public int TransactionId
        {
            get { return _transactionId; }
            set { Set(ref _transactionId, value); }
        }

        private int _batchId;

        public int BatchId
        {
            get { return _batchId; }
            set { Set(ref _batchId, value); }
        }

        private double _quantity;

        public double Quantity
        {
            get { return _quantity; }
            set { Set(ref _quantity, value); }
        }

        private decimal _price;

        public decimal Price
        {
            get { return _price; }
            set { Set(ref _price, value); }
        }

        public Transaction Transaction { get; set; }

        public Batch Batch { get; set; }
    }
}
