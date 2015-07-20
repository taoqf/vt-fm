using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Victop.Frame.PublicLib.Helpers
{
    /// <summary>
    /// Xaml布局树辅助类
    /// </summary>
    public class XamlTreeHelper
    {
        /// 获取父级控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T GetParentObject<T>(DependencyObject obj) where T : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            while (parent != null)
            {
                if (parent is T)
                {
                    return (T)parent;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }

        /// <summary>
        /// 获得子控件(根据Uid)
        /// </summary>
        /// <typeparam name="T">要获得控件类名</typeparam>
        /// <param name="obj">当前控件名</param>
        /// <param name="name">要查询子控件名</param>
        /// <returns>要获得控件类名</returns>
        public static T GetChildObjectByUid<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child = null;
            T grandChild = null;
            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
                if (child is T && (((T)child).Uid == name | string.IsNullOrEmpty(name)))
                {
                    return (T)child;
                }
                else
                {
                    grandChild = GetChildObjectByUid<T>(child, name);
                    if (grandChild != null)
                        return grandChild;
                }
            }
            return null;
        }

        /// <summary>
        /// 获得子控件(根据Name)
        /// </summary>
        /// <typeparam name="T">要获得控件类名</typeparam>
        /// <param name="obj">当前控件名</param>
        /// <param name="name">要查询子控件名</param>
        /// <returns>要获得控件类名</returns>
        public static T GetChildObjectByName<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child = null;
            T grandChild = null;
            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
                if (child is T && (((T)child).Name == name | string.IsNullOrEmpty(name)))
                {
                    return (T)child;
                }
                else
                {
                    grandChild = GetChildObjectByName<T>(child, name);
                    if (grandChild != null)
                        return grandChild;
                }
            }
            return null;
        }

    }
}
