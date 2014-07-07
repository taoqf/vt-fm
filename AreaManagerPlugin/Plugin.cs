using AreaManagerPlugin.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Victop.Server.Controls;

namespace AreaManagerPlugin
{
    public class Plugin:IPlugin
    {
        public string PluginTitle
        {
            get { return "区域管理"; }
        }

        public string PluginName
        {
            get { return "AreaManagerPlugin"; }
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
            get { return 0; }
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
            get { return new UCTestMasterTemplate(); }
        }

        public void Init()
        {
            throw new NotImplementedException();
        }
    }
}
