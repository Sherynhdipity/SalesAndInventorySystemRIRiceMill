using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NittyGritty;

namespace R1RiceMill.ViewModels.Inventory
{
    public class AddProductViewModel : ObservableObject
    {

        private string _variety;

        public string Variety
        {
            get { return _variety; }
            set
            {
                Set(ref _variety, value);
                RaisePropertyChanged(nameof(IsValid));
            }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { Set(ref _description, value); }
        }

        public bool IsValid => !string.IsNullOrWhiteSpace(Variety);

    }
}
