using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;

namespace Victop.Wpf.Controls
{
    public class TreeListViewEventArgs : RoutedEventArgs
    {
        public DataRow CurDataRow { get; set; }

        public DataRowView CurDataRowView { get; set; }
    }
}
