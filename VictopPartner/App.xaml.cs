﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Victop.Frame.CoreLibrary;
using Victop.Frame.DataMessageManager;
using Victop.Frame.PublicLib.Helpers;
using Victop.Frame.PublicLib.Managers;
using Victop.Server.Controls;
using Victop.Wpf.Controls;

namespace VictopPartner
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string errorInfo = string.Empty;
            string alertInfo = string.Empty;
            if (e.Exception.InnerException != null)
            {
                errorInfo = string.Format("异常源:{0},异常位置:{1},异常信息:{2}", e.Exception.InnerException.Source, e.Exception.InnerException.StackTrace, e.Exception.InnerException.Message);
                alertInfo = string.Format("{0}异常位置:{1}", e.Exception.Message, e.Exception.InnerException.StackTrace);
            }
            VicMessageBoxNormal.Show(alertInfo, "未知错误", MessageBoxButton.OK, MessageBoxImage.Error);
            LoggerHelper.Error(errorInfo);
            e.Handled = true;

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            #region 自动更新
            if (!ConfigurationManager.AppSettings["DevelopMode"].Equals("Debug"))
            {
                string[] argsStr = Environment.GetCommandLineArgs();
                if (argsStr.Count() <= 1 || (argsStr.Count() > 1 && !Convert.ToBoolean(argsStr[1].ToString())))
                {
                    Process updatePro = Process.Start(ConfigurationManager.AppSettings["updateconfig"] + ".exe", Process.GetCurrentProcess().Id.ToString());
                    updatePro.WaitForExit();
                }
            }
            #endregion
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
                    MessageBox.Show(ex.Message);
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
            string skinName = ConfigManager.GetAttributeOfNodeByName("UserInfo", "UserSkin");
            string skinNamespace = System.IO.Path.GetFileNameWithoutExtension(string.Format("theme\\{0}.dll", skinName));//得到皮肤命名空间
            this.ChangeFrameWorkTheme("/" + skinNamespace + ";component/Styles.xaml", skinName);
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
            DataMessageOperation messageOp = new DataMessageOperation();
            messageOp.SendMessage(messageType, contentDic);
        }
        #endregion
    }
}
