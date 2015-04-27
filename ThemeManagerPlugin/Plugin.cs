using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ThemeManagerPlugin.Views;
using Victop.Server.Controls;

namespace ThemeManagerPlugin
{
    public class Plugin : IPlugin
    {
        private string pluginTitle = "皮肤管理";
        public string PluginTitle
        {
            get { return pluginTitle; }
            set { pluginTitle = value; }
        }

        public string PluginName
        {
            get { return "ThemeManagerPlugin"; }
        }
        /// 显示类型
        /// 0:窗口
        /// 1:UserControl
        public int ShowType
        {
            get { return 0; }
        }
        /// <summary>
        ///是否为系统插件
        ///0:系统插件
        ///1:非系统插件
        /// </summary>
        public int SystemPlugin
        {
            get { return 0; }
        }

        public int AutoInit
        {
            get { return 0; }
        }

        public void Init()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 起始窗口
        /// </summary>
        public Window StartWindow
        {
            get { return new ThemesWindow(); }
        }
        /// <summary>
        /// 起始用户组件
        /// </summary>
        public UserControl StartControl
        {
            get { throw new NotImplementedException(); }
        }


        public List<string> ServiceReceiptMessageType
        {
            get { return new List<string>(); }
        }

        private Dictionary<string, object> paramDict;
        public Dictionary<string, object> ParamDict
        {
            get
            {
                if (paramDict == null)
                    paramDict = new Dictionary<string, object>();
                return paramDict;
            }
            set
            {
                paramDict = value;
            }
        }
    }
}
