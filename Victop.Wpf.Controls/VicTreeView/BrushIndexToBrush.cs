using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Victop.Wpf.Controls
{
    public class BrushIndexToBrush : IValueConverter
    {
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int num = System.Convert.ToInt32(value, CultureInfo.CurrentCulture);
                List<Brush> list = (List<Brush>)parameter;
                if (num > list.Count)
                {
                    return new SolidColorBrush(Color.FromRgb(0, 0, 0));
                }
                return list[num];
            }
            catch (Exception)
            {
                return new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }
    }
}
