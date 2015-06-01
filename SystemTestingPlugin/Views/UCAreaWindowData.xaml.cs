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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SystemTestingPlugin.Views
{
    /// <summary>
    /// UCAreaWindowData.xaml 的交互逻辑
    /// </summary>
    public partial class UCAreaWindowData : UserControl
    {
        public UCAreaWindowData()
        {
            InitializeComponent();
        }
        private static Dictionary<string, object> paramDict;

        public static Dictionary<string, object> ParamDict
        {
            get { return paramDict; }
            set { paramDict = value; }
        }
    }
}
