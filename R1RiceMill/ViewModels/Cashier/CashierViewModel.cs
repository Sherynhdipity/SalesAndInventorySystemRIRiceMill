using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NittyGritty;
using R1RiceMill.Core;

namespace R1RiceMill.ViewModels.Cashier
{
    public class CashierViewModel : ObservableObject
    {
        public CashierViewModel()
        {

        }

        private User _user;

        public User User
        {
            get { return _user; }
            set { Set(ref _user, value); }
        }

    }
}
