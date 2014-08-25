using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DataCruisePlugin.Views
{
    /// <summary>
    /// CruiseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CruiseWindow : Window
    {
        public CruiseWindow()
        {
            InitializeComponent();
        }
        private static Dictionary<string, object> paramDict;

        public static Dictionary<string, object> ParamDict
        {
            get { return CruiseWindow.paramDict; }
            set { CruiseWindow.paramDict = value; }
        }
    }
}
