using System;
using System.Windows;
using System.Windows.Controls;

namespace Victop.Wpf.Controls
{
    public class RowExpander : Control
    {
        static RowExpander()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(RowExpander), new FrameworkPropertyMetadata(typeof(RowExpander)));
        }
    }
}
