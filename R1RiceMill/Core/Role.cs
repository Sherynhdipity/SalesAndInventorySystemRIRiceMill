using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NittyGritty;

namespace R1RiceMill.Core
{
    public enum Role
    {
        None = 0,
        Administrator = 1,
        Cashier = 2,
        InventoryClerk = 3
    }

    public static class RoleExtensions
    {
        public static Role[] Roles => new Role[] { Role.Administrator, Role.Cashier, Role.InventoryClerk };
    }
}
