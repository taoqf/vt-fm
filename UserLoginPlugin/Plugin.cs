using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls;
using System.Windows;
using System.Windows.Controls;
using UserLoginPlugin.Views;

namespace UserLoginPlugin
{
    public class Plugin:IPlugin
    {
        public string PluginTitle
        {
            get { return "登录"; }
        }

        public string PluginName
        {
            get { return "UserLoginPlugin"; }
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
            get { return new UserLoginWindow(); }
        }

        public UserControl StartControl
        {
            get { throw new NotImplementedException(); }
        }


        public List<string> ServiceReceiptMessageType
        {
            get { return new List<string>(); }
        }
        public Dictionary<string, object> ParamDict
        {
            get
            {
                return new Dictionary<string,object>();
            }
            set
            {
                
            }
        }
    }
}
