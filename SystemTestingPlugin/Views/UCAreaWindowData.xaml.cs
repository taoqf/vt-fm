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
using Victop.Frame.CmptRuntime;

namespace SystemTestingPlugin.Views
{
    /// <summary>
    /// UCAreaWindowData.xaml 的交互逻辑
    /// </summary>
    public partial class UCAreaWindowData : TemplateControl
    {
        public UCAreaWindowData()
        {
            InitializeComponent();
        }

        public static Dictionary<string, object> ParamDict { get; internal set; }
    }
}
