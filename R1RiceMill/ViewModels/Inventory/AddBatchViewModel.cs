using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NittyGritty;

namespace R1RiceMill.ViewModels.Inventory
{
    public class AddBatchViewModel : ObservableObject
    {

        private DateTime? _millingDate;

        public DateTime? MillingDate
        {
            get { return _millingDate; }
            set
            {
                Set(ref _millingDate, value);
                RaisePropertyChanged(nameof(IsValid));
            }
        }

        private decimal? _price;

        public decimal? Price
        {
            get { return _price; }
            set
            {
                Set(ref _price, value);
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

        public bool IsValid => MillingDate <= DateTime.Now && Price > 0 && Quantity > 0;

    }
}
