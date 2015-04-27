using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using MetroUserLoginPlugin.Views;
using Victop.Server.Controls;

namespace MetroUserLoginPlugin
{
    public class Plugin : IPlugin
    {
        private string pluginTitle = "Win8登录";
        public string PluginTitle
        {
            get { return pluginTitle; }
            set { pluginTitle = value; }
        }

        public string PluginName
        {
            get { return "MetroUserLoginPlugin"; }
        }

        public int ShowType
        {
            get { return 0; }
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
            get { return new MetroUserLoginPlugin.Views.UserLoginWindow(); }
        }

        public UserControl StartControl
        {
            get { return new UCUserLogin(); }
        }


        public List<string> ServiceReceiptMessageType
        {
            get { return new List<string>(); }
        }
        public Dictionary<string, object> ParamDict
        {
            get
            {
                return new Dictionary<string, object>();
            }
            set
            {

            }
        }
    }
}
