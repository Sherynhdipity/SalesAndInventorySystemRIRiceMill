using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R1RiceMill.Core
{
    public enum Discount
    {
        None = 0,

        [Description("Loyalty (10%)")]
        Loyalty = 1,

        [Description("Senior Citizen (20%)")]
        SeniorCitizen = 2,

        [Description("PWD (20%)")]
        PWD = 3
    }

    public static class DiscountExtensions
    {
        public static Discount[] Discounts => Enum.GetValues<Discount>();

        public static decimal ComputeDiscount(this Discount discount, decimal subTotal)
        {
            switch (discount)
            {
                case Discount.Loyalty:
                    return subTotal * 0.1M;

                case Discount.SeniorCitizen:
                    return subTotal * 0.2M;

                case Discount.PWD:
                    return subTotal * 0.2M;

                case Discount.None:
                default:
                    return 0M;
            }
        }
    }
}
