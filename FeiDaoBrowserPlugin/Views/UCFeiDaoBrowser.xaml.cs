using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Victop.Frame.CmptRuntime;
using Victop.Frame.PublicLib.Helpers;

namespace FeiDaoBrowserPlugin.Views
{
    /// <summary>
    /// UCFeiDaoBrowser.xaml 的交互逻辑
    /// </summary>
    public partial class UCFeiDaoBrowser : TemplateControl
    {
        private bool firstLoaded = true;
        public UCFeiDaoBrowser(Dictionary<string, object> paramDict, int showType)
        {
            InitializeComponent();
            this.DataContext = this;
            ParamDict = paramDict;
            ShowType = showType;
        }

        private void mainView_Loaded(object sender, RoutedEventArgs e)
        {
            if (firstLoaded && ParamDict != null && ParamDict.ContainsKey("formid"))
            {
                firstLoaded = false;
                string url = ParamDict["formid"].ToString();
                if (url.EndsWith("="))
                {
                    string ticket = FeiDaoOp.GetCurrentUserSSOTicket(null);
                    url = string.Format("{0}{1}", ParamDict["formid"].ToString(), ticket);
                }
                feidaoBrowser.Navigate(url);
            }
        }
        private void feidaoBrowser_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            SuppressScriptErrors((WebBrowser)sender, true);
        }
        #region 私有方法
        private void SuppressScriptErrors(WebBrowser webBrowser, bool Hide)
        {
            FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;

            object objComWebBrowser = fiComWebBrowser.GetValue(webBrowser);
            if (objComWebBrowser == null) return;

            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { Hide });
        }
        #endregion
    }
}
