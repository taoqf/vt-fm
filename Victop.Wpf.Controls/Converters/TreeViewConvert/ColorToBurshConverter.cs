using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Victop.Wpf.Controls
{
    public class ColorToBurshConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return new SolidColorBrush((Color)value);
            }
            return new SolidColorBrush(Color.FromRgb(0xda, 0xdf, 0xe7));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
