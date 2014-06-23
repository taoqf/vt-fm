using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Data;

namespace Victop.Wpf.Controls
{
    public class LevelToIndentConverter : IValueConverter
    {
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults 不要忽略方法结果")]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double result = 0.0;
            if (parameter != null)
            {
                double.TryParse(parameter.ToString(), out result);
            }
            return (((int)value) * result);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
