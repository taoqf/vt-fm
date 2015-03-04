using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MetroFramePlugin
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
                    return Application.Current.Resources["UserPictrue"] as ImageBrush;
                }

                ImageBrush image = new ImageBrush();
                image.Stretch = Stretch.UniformToFill;
                image.ImageSource = new BitmapImage(new Uri(value.ToString()));
                return image;
            }
            catch
            {
                return Application.Current.Resources["UserPictrue"] as ImageBrush;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
