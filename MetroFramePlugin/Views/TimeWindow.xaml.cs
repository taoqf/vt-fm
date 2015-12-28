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

namespace MetroFramePlugin.Views
{
    /// <summary>
    /// TimeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TimeWindow : VicWindowNormal
    {
        public TimeWindow()
        {
            InitializeComponent();
            web.UnitUCWebBrowserParams.URL = "http://192.168.40.60:8087/countdown/countdate.html";
            web.wBrowser_LoadCompleted();
        }
    }
}
