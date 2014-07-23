﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace PortalFramePlugin
{
    /// <summary>
    /// 转换器。
    /// </summary>
    public class ImgeNameToImageUrlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || value.ToString() == string.Empty) return string.Empty;
            return new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"images/"+value.ToString()));
           
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}