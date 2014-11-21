using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Victop.Frame.PublicLib.Helpers;
using Victop.Frame.CoreLibrary;
using Victop.Frame.CoreLibrary.Models;
using Victop.Server.Controls;
using System.Windows.Media.Animation;
using System.Configuration;
using System.IO;
using Victop.Frame.SyncOperation;

namespace VictopPartner
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string AnonymousLogin()
        {
            Dictionary<string, string> messageDic = new Dictionary<string, string>();
            messageDic.Add("MessageType", "LoginService.getCurrentLinker");
            Dictionary<string, string> contentDic = new Dictionary<string, string>();
            contentDic.Add("usercode", "test7");
            messageDic.Add("MessageContent", JsonHelper.ToJson(contentDic));
            return JsonHelper.ToJson(messageDic);
        }

        void mainstory_Completed(object sender, EventArgs e)
        {
            if (FrameInit.GetInstance().FrameRun())
            {
                try
                {
                    this.GetCurrentSkin();
                    string mainPlugin = ConfigurationManager.AppSettings["portalWindow"];
                    Assembly pluginAssembly = ServerFactory.GetServerAssemblyByName(mainPlugin, "");
                    Type[] types = pluginAssembly.GetTypes();
                    foreach (Type t in types)
                    {
                        if (IsValidPlugin(t))
                        {
                            IPlugin plugin = (IPlugin)pluginAssembly.CreateInstance(t.FullName);
                            this.Hide();
                            plugin.StartWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                            plugin.StartWindow.ShowDialog();
                            FrameInit.GetInstance().FrameUnload();
                            Environment.Exit(0);
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("门户" + ex.Message);
                    FrameInit.GetInstance().FrameUnload();
                    Environment.Exit(0);
                }
            }
        }

        /// <summary>
        /// 是否为有效的插件
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool IsValidPlugin(Type type)
        {
            bool ret = false;
            Type[] interfaces = type.GetInterfaces();
            foreach (Type theInterface in interfaces)
            {
                if (theInterface.FullName == "Victop.Server.Controls.IPlugin")
                {
                    ret = true;
                    break;
                }
            }
            return ret;
        }

        #region 换肤

        /// <summary>
        /// 获取当前皮肤
        /// </summary>
        private void GetCurrentSkin()
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains("skinurl") && string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("skinurl")) == false)
            {
                string skinNamespace = System.IO.Path.GetFileNameWithoutExtension(ConfigurationManager.AppSettings.Get("skinurl"));//得到皮肤命名空间
                this.ChangeFrameWorkTheme("/" + skinNamespace + ";component/Styles.xaml", ConfigurationManager.AppSettings.Get("skinurl"));
            }
        }

        /// <summary>
        /// 主题皮肤改变发送消息
        /// </summary>
        private void ChangeFrameWorkTheme(string ThemeName, string SkinPath)
        {
            string messageType = "ServerCenterService.ChangeThemeByDll";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            Dictionary<string, string> ServiceParams = new Dictionary<string, string>();
            ServiceParams.Add("SourceName", ThemeName);
            ServiceParams.Add("SkinPath", SkinPath);
            contentDic.Add("ServiceParams", JsonHelper.ToJson(ServiceParams));
            MessageOperation messageOp = new MessageOperation();
            messageOp.SendMessage(messageType, contentDic);
        }
        #endregion
    }
}
