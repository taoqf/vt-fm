using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Victop.Wpf.Controls
{
    public class VicNumericUpDownChangedRoutedEventArgs : RoutedEventArgs
    {
        public double Interval { get; set; }

        public VicNumericUpDownChangedRoutedEventArgs(RoutedEvent routedEvent, double interval)
            : base(routedEvent)
        {
            Interval = interval;
        }
    }
}
