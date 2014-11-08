using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Victop.Server.Controls;
using MachinePlatformPlugin.Views;
using System.Windows.Controls;
namespace MachinePlatformPlugin
{
    public class Plugin : IPlugin
    {
        public string PluginTitle
        {
            get { return "页面流程机台 "; }
        }

        public string PluginName
        {
            get { return "MachinePlatformPlugin"; }
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
            get { return new MachinePlatform(); }
        }

        public UserControl StartControl
        {
            get { return new UCMachinePlatform(); }
        }

        public void Init()
        {
            throw new NotImplementedException();
        }


        public Dictionary<string, object> ParamDict
        {
            get
            {
                if (ShowType == 0)
                {
                    return MachinePlatform.ParamDict;
                }
                else
                {
                    return UCMachinePlatform.ParamDict;
                }
            }
            set
            {
                UCMachinePlatform.ParamDict = value;
                MachinePlatform.ParamDict = value;
            }
        }
    }
}
