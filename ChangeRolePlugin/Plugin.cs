using ChangeRolePlugin.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Victop.Server.Controls;

namespace ChangeRolePlugin
{
    public class Plugin : IPlugin
    {
        private string pluginTitle = "切换角色";
        public string PluginTitle
        {
            get { return pluginTitle; }
            set { pluginTitle = value; }
        }

        public string PluginName
        {
            get { return "ChangeRolePlugin"; }
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
            get { return new ChangeRoleWindow(); }
        }

        public UserControl StartControl
        {
            get { return new UCChangeRole(); }
        }


        public List<string> ServiceReceiptMessageType
        {
            get { return new List<string>(); }
        }
        public Dictionary<string, object> ParamDict
        {
            get
            {
                if (ShowType == 0)
                {
                    return ChangeRoleWindow.ParamDict;
                }
                else
                {
                    return UCChangeRole.ParamDict;
                }
            }
            set
            {
                UCChangeRole.ParamDict = value;
                ChangeRoleWindow.ParamDict = value;
                UCChangeRole.ShowType = this.ShowType;
                ChangeRoleWindow.ShowType = this.ShowType;
            }
        }

    }
}
