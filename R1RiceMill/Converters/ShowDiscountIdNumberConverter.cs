using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using R1RiceMill.Core;

namespace R1RiceMill.Converters
{
    public class ShowDiscountIdNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var e = (Discount)value;
            return e == Discount.SeniorCitizen || e == Discount.PWD ?
                Visibility.Visible :
                Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
