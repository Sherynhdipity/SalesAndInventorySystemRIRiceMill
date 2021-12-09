using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NittyGritty;
using R1RiceMill.Core;

namespace R1RiceMill.ViewModels.Admin
{
    public class ChangeBatchPriceViewModel : ObservableObject
    {
        public ChangeBatchPriceViewModel(Batch batch, decimal originalPrice)
        {
            Batch = batch;
            OriginalPrice = originalPrice;
        }

        public Batch Batch { get; }

        public decimal OriginalPrice { get; }

        private decimal? _newPrice;

        public decimal? NewPrice
        {
            get { return _newPrice; }
            set
            {
                Set(ref _newPrice, value);
                RaisePropertyChanged(nameof(IsValid));
            }
        }

        public bool IsValid => NewPrice > 0;

    }
}
