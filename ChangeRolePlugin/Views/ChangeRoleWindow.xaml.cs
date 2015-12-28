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

namespace ChangeRolePlugin.Views
{
    /// <summary>
    /// ChangeRoleWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ChangeRoleWindow : Window
    {
        public ChangeRoleWindow()
        {
            InitializeComponent();
        }
        private static Dictionary<string, object> paramDict;

        public static Dictionary<string, object> ParamDict
        {
            get {
                return ChangeRoleWindow.paramDict;
            }
            set { ChangeRoleWindow.paramDict = value; }
        }
        private static int _showType;

        public static int ShowType
        {
            get { return ChangeRoleWindow._showType; }
            set { ChangeRoleWindow._showType = value; }
        }
    }
}
