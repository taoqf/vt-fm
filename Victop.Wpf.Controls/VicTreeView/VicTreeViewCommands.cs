using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Victop.Wpf.Controls
{
    public static class VicTreeViewCommands
    {
        private static RoutedCommand _clearFilterCommand = new RoutedCommand();
        private static RoutedCommand _DownSearchCommand = new RoutedCommand();
        private static RoutedCommand _UpSearchCommand = new RoutedCommand();

        public static RoutedCommand ClearFilter
        {
            get
            {
                return _clearFilterCommand;
            }
        }

        public static RoutedCommand DownSearch
        {
            get
            {
                return _DownSearchCommand;
            }
        }

        public static RoutedCommand UpSearch
        {
            get
            {
                return _UpSearchCommand;
            }
        }
    }
}
