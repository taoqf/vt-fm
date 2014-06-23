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
using Victop.Frame.MessageManager;
using Victop.Frame.PublicLib.Helpers;
using Victop.Frame.CoreLibrary;
using Victop.Frame.CoreLibrary.Models;
using Victop.Server.Controls;

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
        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            PluginRun();
        }
        /// <summary>
        /// 插件启动
        /// </summary>
        private void PluginRun()
        {
            Dictionary<string, string> messageDic = new Dictionary<string, string>();
            messageDic.Add("MessageType", "PluginService.PluginRun");
            Dictionary<string, string> contentDic = new Dictionary<string, string>();
            contentDic.Add("PluginName", "PortalFramePlugin");
            contentDic.Add("PluginPath", "");
            messageDic.Add("MessageContent", JsonHelper.ToJson(contentDic));
           new  PluginMessage().SendMessage(Guid.NewGuid().ToString(), JsonHelper.ToJson(messageDic), new WaitCallback(Test));
        }

        /// <summary>
        /// 插件回调
        /// </summary>
        /// <param name="newTest"></param>
        private void Test(object newTest)
        {
                ActivePluginManager pluginManager = new ActivePluginManager();
                ActivePluginInfo pluginInfo = pluginManager.GetActivePlugins()[JsonHelper.ReadJsonString(newTest.ToString(), "MessageId")];
                System.Windows.Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new WaitCallback(DoWork), pluginInfo);
            }
        /// <summary>
        /// 打开插件
        /// </summary>
        /// <param name="pluginInfo"></param>
        private void DoWork(object pluginInfo)
        {
            ActivePluginInfo plugin = (ActivePluginInfo)pluginInfo;
            switch (plugin.ShowType)
            {
                case 0:
                    Window win = ((IPlugin)((ActivePluginInfo)pluginInfo).PluginInstance).StartWindow;
                    win.Uid = ((ActivePluginInfo)pluginInfo).ObjectId;
                    win.Show();
                    break;
                case 1:
                    UserControl uctrl = ((IPlugin)((ActivePluginInfo)pluginInfo).PluginInstance).StartControl;
                    uctrl.Uid = plugin.ObjectId;
                    pluginGrid.Children.Add(uctrl);
                    break;
                default:
                    break;
            }
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            FrameInit.GetInstance().FrameRun();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            new GalleryManager().SetCurrentGalleryId(Victop.Frame.CoreLibrary.Enums.GalleryEnum.ENTERPRISE);
            string userloginstr = UserLoginByInfo();//常规登录
            //string userloginstr = AnonymousLogin();
            //string userloginstr = GetGalleryInfo();
            new PluginMessage().SendMessage(Guid.NewGuid().ToString(), userloginstr, new WaitCallback(Login));
        }

        private string GetGalleryInfo()
        {
            Dictionary<string, string> messageDic = new Dictionary<string, string>();
            messageDic.Add("MessageType", "GalleryService.GetGalleryInfo");
            Dictionary<string, string> contentDic = new Dictionary<string, string>();
            contentDic.Add("usercode", "test7");
            messageDic.Add("MessageContent", JsonHelper.ToJson(contentDic));
            string gallerystr = JsonHelper.ToJson(messageDic);
            return gallerystr;
        }

        private string UserLoginByInfo()
        {
            Dictionary<string, string> messageDic = new Dictionary<string, string>();
            messageDic.Add("MessageType", "PluginService.PluginRun");
            Dictionary<string, string> contentDic = new Dictionary<string, string>();
            contentDic.Add("PluginName", "UserLoginPlugin");
            contentDic.Add("PluginPath", "");
            messageDic.Add("MessageContent", JsonHelper.ToJson(contentDic));
            string userloginstr = JsonHelper.ToJson(messageDic);
            return userloginstr;
        }
        private string AnonymousLogin()
        {
            Dictionary<string, string> messageDic = new Dictionary<string, string>();
            messageDic.Add("MessageType", "LoginService.getCurrentLinker");
            Dictionary<string, string> contentDic = new Dictionary<string, string>();
            contentDic.Add("usercode", "");
            contentDic.Add("userpw", "");
            contentDic.Add("clientId", "");
            messageDic.Add("MessageContent", JsonHelper.ToJson(contentDic));
            return JsonHelper.ToJson(messageDic);
        }
        private void Login(object message)
        {
            if (JsonHelper.ReadJsonString(message.ToString(), "ReplyMode").Equals("0"))
            {
                ActivePluginManager pluginManager = new ActivePluginManager();
                ActivePluginInfo pluginInfo = pluginManager.GetActivePlugins()[JsonHelper.ReadJsonString(message.ToString(), "MessageId")];
                switch (pluginInfo.ShowType)
                {
                    case 0:
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new WaitCallback(DoWork), pluginInfo);
                        break;
                    case 1:
                        break;
                    default:
                        break;
                }
            }
            else
            {
                MessageBox.Show(JsonHelper.ReadJsonString(message.ToString(), "ReplyAlertMessage"));
            }
        }
    }
}
