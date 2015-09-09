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

namespace ModifyPassWordPlugin.Views
{
    /// <summary>
    /// UCModifyPassWord.xaml 的交互逻辑
    /// </summary>
    public partial class UCModifyPassWord : UserControl
    {
        public UCModifyPassWord()
        {
            InitializeComponent();
        }
        private static Dictionary<string, object> paramDict;

        public static Dictionary<string, object> ParamDict
        {
            get { return UCModifyPassWord.paramDict; }
            set { UCModifyPassWord.paramDict = value; }
        }

        private static int _showType;

        public static int ShowType
        {
            get { return UCModifyPassWord._showType; }
            set { UCModifyPassWord._showType = value; }
        }
        
    }
}
