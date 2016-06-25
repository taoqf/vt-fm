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
using Victop.Frame.CmptRuntime;
using Victop.Wpf.Controls;

namespace FeiDaoBrowserPlugin.Views
{
    /// <summary>
    /// UCFeiDaoBrowserWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UCFeiDaoBrowserWindow : VicMetroWindow
    {
        public UCFeiDaoBrowserWindow(Dictionary<string, object> paramDict, int showType)
        {
            InitializeComponent();
            UCFeiDaoBrowser mainView = new UCFeiDaoBrowser(paramDict, showType);
            mainView.Uid = Uid;
            this.Closing += UCFeiDaoBrowserWindow_Closing;
            this.Content = mainView;
        }

        private void UCFeiDaoBrowserWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TemplateControl tctrl = Content as TemplateControl;
            if (tctrl != null)
            {
                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("MessageType", "WPFClear");
                tctrl.Excute(paramDic);
            }
        }
    }
}
