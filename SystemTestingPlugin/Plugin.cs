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
            get { return new AreaWindow(ParamDict,ShowType); }
        }

        public UserControl StartControl
        {
            get
            {
                //return new UCTemplateControlDemo();
                return new UCAreaWindowData(ParamDict,ShowType);
            }
        }

        public void Init()
        {
        }

        public Dictionary<string, object> ParamDict
        {
            get; set;
        }
    }
}
