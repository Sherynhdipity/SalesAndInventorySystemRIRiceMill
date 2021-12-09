using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NittyGritty;
using NittyGritty.Commands;
using NittyGritty.Models;
using R1RiceMill.Core;

namespace R1RiceMill.ViewModels.Admin
{
    public class AdminViewModel : ObservableObject
    {
        public AdminViewModel()
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
