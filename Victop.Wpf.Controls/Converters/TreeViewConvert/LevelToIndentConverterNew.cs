using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Victop.Wpf.Controls
{
    internal class LevelToIndentConverterNew : IValueConverter
    {
        private const double IndentSize = 10.0;

        public object Convert(object o, Type type, object parameter, CultureInfo culture)
        {
            double left = 0.0;
            UIElement element = o as TreeViewItem;
            while (element.GetType() != typeof(VicTreeView))
            {
                element = (UIElement)VisualTreeHelper.GetParent(element);
                if (element.GetType() == typeof(TreeViewItem))
                    left += IndentSize;
            }
            return new Thickness(left, 0, 0, 0);
        }

        public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
