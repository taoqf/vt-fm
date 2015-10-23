using AutomaticCodePlugin.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Victop.Server.Controls;

namespace AutomaticCodePlugin
{
    public class Plugin:IPlugin
    {
        private string pluginTitle = "自动代码插件";
        public string PluginTitle
        {
            get { return pluginTitle; }
            set { pluginTitle = value; }
        }

        public string PluginName
        {
            get { return "AutomaticCodePlugin"; }
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
            get { return new MainViewWindow(); }
        }

        public UserControl StartControl
        {
            get
            {
                return new UCMainView();
            }
        }

        public void Init()
        {
        }

        public Dictionary<string, object> ParamDict
        {
            get
            {
                return UCMainView.ParamDict;

            }
            set
            {
                UCMainView.ParamDict = value;
            }
        }
    }
}
