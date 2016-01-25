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
        public ModifyPassWordWindow(Dictionary<string, object> paramDict, int showType)
        {
            InitializeComponent();
            ucModifyPassWord.ParamDict = paramDict;
            ucModifyPassWord.ShowType = showType;
        }
    }
}
