using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Victop.Frame.CmptRuntime;

namespace Victop.Frame.AtomicOperation.UIAtOperation
{
    /// <summary>
    /// 界面元素操作
    /// </summary>
    public static class ViewElementOperation
    {
        /// <summary>
        /// 设置Button是否可用
        /// </summary>
        /// <param name="viewValue">Button所属界面值</param>
        /// <param name="btnName">Button名称</param>
        /// <param name="isEnable">是否可用,true:可以;false:不可用</param>
        public static void SetButtonEnable(TemplateControl viewValue, string btnName, bool isEnable)
        {
            if (viewValue != null)
            {
                Button btn = viewValue.FindName(btnName) as Button;
                if (btn != null)
                {
                    btn.IsEnabled = isEnable;
                }
            }
        }
        /// <summary>
        /// 设置按钮显示状态
        /// </summary>
        /// <param name="viewValue">Button所属界面值</param>
        /// <param name="btnName">Button名称</param>
        /// <param name="isEnable">是否可用,true:可见;false:隐藏</param>
        public static void SetButtonVisibility(TemplateControl viewValue, string btnName, bool isEnable)
        {
            if (viewValue != null)
            {
                Button btn = viewValue.FindName(btnName) as Button;
                if (btn != null)
                {
                    btn.Visibility = isEnable ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }
    }
}
