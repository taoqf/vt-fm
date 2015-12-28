using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Victop.Wpf.Controls;

namespace ModifyPassWordPlugin.Views
{
    /// <summary>
    /// ModifyPassWordWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ModifyPassWordWindow : VicWindowNormal
    {
        public ModifyPassWordWindow()
        {
            InitializeComponent();
        }
        private static Dictionary<string, object> paramDict;

        public static Dictionary<string, object> ParamDict
        {
            get { return ModifyPassWordWindow.paramDict; }
            set { ModifyPassWordWindow.paramDict = value; }
        }

        private static int _showType;

        public static int ShowType
        {
            get { return ModifyPassWordWindow._showType; }
            set { ModifyPassWordWindow._showType = value; }
        }
    }
}
