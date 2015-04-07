using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace MetroUserLoginPlugin.Converts
{
    /// <summary>
    /// CheckBox从整型转换为bool类型（前提是0：否 1：是）
    /// </summary>
    public class CheckboxBoolConverter : IValueConverter
    {
        #region 绑定
        /// <summary>
        /// 获取数据源绑定指向目标时调用此转换
        /// </summary>
        /// <param name="value">数据值</param>
        /// <param name="targetType">类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                int isStop = int.Parse(value.ToString());
                switch (isStop)
                {
                    case 0:
                        return false;
                    case 1:
                        return true;
                    default:
                        return "";
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        #endregion

        #region 回调
        /// <summary>
        /// 指向目标改变向数据库中提交时调用此转换
        /// </summary>
        /// <param name="value">数据值</param>
        /// <param name="targetType">类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                bool? isChecked = (bool?)value;
                switch (isChecked)
                {
                    case true:
                        return 1;
                    case false:
                        return 0;
                    case null:
                        return "";
                    default:
                        return "";
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        #endregion
    }
}
