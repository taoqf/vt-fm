using ModifyPassWordPlugin.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Victop.Server.Controls;

namespace ModifyPassWordPlugin
{
    public class Plugin:IPlugin
    {
         private string pluginTitle = "修改密码";
        public string PluginTitle
        {
            get { return pluginTitle; }
            set { pluginTitle = value; }
        }

        public string PluginName
        {
            get { return "ModifyPassWordPlugin"; }
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
            get { return new ModifyPassWordWindow(); }
        }

        public UserControl StartControl
        {
            get { return new UCModifyPassWord(); }
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
                    return ModifyPassWordWindow.ParamDict;
                }
                else
                {
                    return UCModifyPassWord.ParamDict;
                }
            }
            set
            {
                UCModifyPassWord.ParamDict = value;
                ModifyPassWordWindow.ParamDict = value;
                UCModifyPassWord.ShowType = this.ShowType;
                ModifyPassWordWindow.ShowType = this.ShowType;
            }
        }
    
    }
}
