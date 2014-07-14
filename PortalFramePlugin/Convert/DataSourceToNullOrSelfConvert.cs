using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using PortalFramePlugin.Models;

namespace PortalFramePlugin
{
    /// <summary>
    /// 转换器。
    /// </summary>
    public class DataSourceToNullOrSelfConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null||((ObservableCollection <MenuModel>)value).Count==0) return null;
            ObservableCollection<MenuModel> newValue = new ObservableCollection<MenuModel>();
            foreach (MenuModel model in (ObservableCollection<MenuModel>)value)
            {
                if (model.SystemMenuList != null && model.SystemMenuList.Count ==1)
                {
                    if (model.SystemMenuList[0].SystemMenuList != null && model.SystemMenuList[0].SystemMenuList.Count != 0)
                    {
                        newValue.Add(model);
                    }
                }
            }
            return newValue;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
