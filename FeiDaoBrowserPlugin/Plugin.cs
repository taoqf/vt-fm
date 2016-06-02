using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls;
using FeiDaoBrowserPlugin.Views;
using System.Windows;
using System.Windows.Controls;
using FeiDaoBrowserPlugin.Views;

namespace FeiDaoBrowserPlugin
{
    public class Plugin : IPlugin
    {
        private string pluginTitle = "飞道浏览器插件";
        public string PluginTitle
        {
            get { return pluginTitle; }
            set { pluginTitle = value; }
        }

        public string PluginName
        {
            get { return "FeiDaoBrowserPlugin"; }
        }

        public int ShowType
        {
            get { return 1; }
        }

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


        public Window StartWindow
        {
            get { return new UCFeiDaoBrowserWindow(ParamDict, ShowType); }
        }

        public UserControl StartControl
        {
            get { return new UCFeiDaoBrowser(ParamDict, ShowType); }
        }


        public List<string> ServiceReceiptMessageType
        {
            get { return new List<string>(); }
        }
        public Dictionary<string, object> ParamDict
        {
            get;set;
        }
    }
}
