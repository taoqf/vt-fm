using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace MachinePlatformPlugin.Converts
{
    public class StyleConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (int.Parse(parameter.ToString()) == 1)
            {
                if ((bool)value)
                {
                    return FontWeights.Bold;
                }
                else
                {
                    return FontWeights.Normal;
                }
            }
            else
            {
                if ((bool)value)
                {
                    return FontWeights.Normal;
                }
                else
                {
                    return FontWeights.Bold;
                }
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
