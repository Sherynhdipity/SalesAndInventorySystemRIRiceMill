using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NittyGritty;

namespace R1RiceMill.ViewModels.Cashier
{
    public class PaymentViewModel : ObservableObject
    {
        public PaymentViewModel(decimal grandTotal)
        {
            GrandTotal = grandTotal;
        }

        private decimal? _payment;

        public decimal? Payment
        {
            get { return _payment; }
            set
            {
                Set(ref _payment, value);
                RaisePropertyChanged(nameof(IsValid));
            }
        }

        public decimal GrandTotal { get; }

        public bool IsValid => Payment >= GrandTotal;
    }
}
