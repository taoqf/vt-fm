using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Victop.Wpf.Controls
{
    /// <summary>
    /// 转换器。功能：使CheckBox的IsChecked属性和DataGrid控件中的列的Visibility属性可以关联。
    /// </summary>
    [ValueConversion(typeof(Enum), typeof(bool?))]
    public class ColumnVisibilityToBoolConverter : IValueConverter
    {
        /// <summary>
        /// 将DataGrid列的Visibility属性转换为bool型，方便CheckBox的IsChecked属性赋值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string str = System.Convert.ToString(value);
            if (str.Equals("Visible"))
                return true;
            else
                return false;
        }
        /// <summary>
        /// 将CheckBox的IsChecked属性的bool值转换成Visibility属性，方便DataGrid列属性赋值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool b = System.Convert.ToBoolean(value);
            if (b.Equals(true))
            {
                return (Visibility)Enum.Parse(typeof(Visibility), "Visible", true);
            }
            else
                return (Visibility)Enum.Parse(typeof(Visibility), "Hidden", true);
        }
    }
}
