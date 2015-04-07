using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MetroUserLoginPlugin.Converts
{
    /// <summary>
    /// 转换器。
    /// </summary>
    public class ImgeNameToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value == null || value.ToString() == string.Empty)
                {
                    return null;
                }

                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\VictopPartner\\UserPhoto\\" + value.ToString() + ".jpg";
                if (System.IO.File.Exists(path) == false)
                {
                    return null;
                }

                ImageSource image = new BitmapImage(new Uri(path));
                return image;
            }
            catch
            {
                return null;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
