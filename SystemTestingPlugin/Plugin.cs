using SystemTestingPlugin.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Victop.Server.Controls;

namespace SystemTestingPlugin
{
    public class Plugin : IPlugin
    {
        private string pluginTitle = "测试数据工具";
        public string PluginTitle
        {
            get { return pluginTitle; }
            set { pluginTitle = value; }
        }

        public string PluginName
        {
            get { return "SystemTestingPlugin"; }
        }

        public List<string> ServiceReceiptMessageType
        {
            get { return new List<string>(); }
        }

        public int ShowType
        {
            get { return 1; }
        }

        public int SystemPlugin
        {
            get { return 1; }
        }

        public int AutoInit
        {
            get { return 1; }
        }

        public Window StartWindow
        {
            get { return new AreaWindow(); }
        }

        public UserControl StartControl
        {
            get
            {
                //return new UCAreaWindow();
                //return new UCSimpleDefWindow();
                return new UCAreaWindowData();
                //return new UCVictopFTP();
            }
        }

        public void Init()
        {
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
