using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Humanizer;

namespace R1RiceMill.Converters
{
    public class HumanizerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case Enum e:
                    return e.Humanize();
                case string s:
                    return s.Humanize();
                case DateTimeOffset dto:
                    return dto.Humanize();
                default:
                    return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
