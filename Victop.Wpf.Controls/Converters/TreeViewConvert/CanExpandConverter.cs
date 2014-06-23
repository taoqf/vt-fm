using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Victop.Wpf.Controls
{
    internal class CanExpandConverter : IValueConverter
    {
        public object Convert(object o, Type type, object parameter, CultureInfo culture)
        {
            if ((bool)o)
            {
                return Visibility.Visible;
            }
            return Visibility.Hidden;
        }

        public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
