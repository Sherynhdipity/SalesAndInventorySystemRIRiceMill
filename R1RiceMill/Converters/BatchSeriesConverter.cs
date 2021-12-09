using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using R1RiceMill.Core;

namespace R1RiceMill.Converters
{
    public class BatchSeriesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IList<Batch> batches)
            {
                return new List<ISeries>
                {
                    new ColumnSeries<double>
                    {
                        Values = batches.Select(b => b.Quantity),
                        GroupPadding = 0.5,
                        MaxBarWidth = 10
                    }
                };
            }
            else
            {
                return Enumerable.Empty<ISeries>();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
