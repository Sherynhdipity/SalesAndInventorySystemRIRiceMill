using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NittyGritty;

namespace R1RiceMill.Core
{
    public class Transaction : ObservableObject
    {

        private int _id;

        public int Id
        {
            get { return _id; }
            set { Set(ref _id, value); }
        }

        private long _transactionNumber;

        public long TransactionNumber
        {
            get { return _transactionNumber; }
            set { Set(ref _transactionNumber, value); }
        }

        private string _invoiceCode;

        public string InvoiceCode
        {
            get { return _invoiceCode; }
            set { Set(ref _invoiceCode, value); }
        }

        private DateTime _date;

        public DateTime Date
        {
            get { return _date; }
            set { Set(ref _date, value); }
        }

        private int _userId;

        public int UserId
        {
            get { return _userId; }
            set { Set(ref _userId, value); }
        }

        private int _customerId;

        public int CustomerId
        {
            get { return _customerId; }
            set { Set(ref _customerId, value); }
        }

        private Discount _discount;

        public Discount Discount
        {
            get { return _discount; }
            set { Set(ref _discount, value); }
        }

        private string _discountIdNumber;

        public string DiscountIdNumber
        {
            get { return _discountIdNumber; }
            set { Set(ref _discountIdNumber, value); }
        }

        private decimal _payment;

        public decimal Payment
        {
            get { return _payment; }
            set { Set(ref _payment, value); }
        }

        public User User { get; set; }

        public Customer Customer { get; set; }

        private ObservableCollection<Order> orders;
        public ObservableCollection<Order> Orders { get => orders ?? (orders = new ObservableCollection<Order>()); set => orders = value; }

        private static readonly Random random = new Random();

        public static long GenerateTransactionNumber(DateTime date)
        {
            var gen = $"{date:yyyyMMddHHmmss}{random.Next(0, 100_000):00000}";
            return long.Parse(gen);
        }

        public static string GenerateInvoiceCode(long transactionNumber)
        {
            using (var md5 = MD5.Create())
            {
                var text = Encoding.UTF8.GetBytes($"{transactionNumber}");
                var hash = md5.ComputeHash(text);
                return BitConverter.ToString(hash).Replace("-", string.Empty);
            }
        }

    }
}
