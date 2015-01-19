using System;
using System.Collections.Generic;
using System.Data;
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
using SystemTestingPlugin.Models;
using Victop.Frame.DataMessageManager;
using Victop.Frame.PublicLib.Helpers;

namespace SystemTestingPlugin.Views
{
    /// <summary>
    /// UCUniversalRefWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UCUniversalRefWindow : UserControl
    {
        private static RefDataModel refDataInfo;

        public static RefDataModel RefDataInfo
        {
            get { return UCUniversalRefWindow.refDataInfo; }
            set { UCUniversalRefWindow.refDataInfo = value; }
        }
        public UCUniversalRefWindow(RefDataModel refDataInfo)
        {
            InitializeComponent();
            RefDataInfo = refDataInfo;
        }
        
    }
}
