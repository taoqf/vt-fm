using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using MetroFramePlugin.Views;
using Victop.Server.Controls;

namespace MetroFramePlugin
{
    public class Plugin : IPlugin
    {
        public string PluginTitle
        {
            get { return "自定义机台平台"; }
        }

        public string PluginName
        {
            get { return "MetroFramePlugin"; }
        }
        /// 显示类型
        /// 0:窗口
        /// 1:UserControl
        public int ShowType
        {
            get { return 0; }
        }
        /// <summary>
        ///是否为系统插件
        ///0:系统插件
        ///1:非系统插件
        /// </summary>
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
        /// <summary>
        /// 起始窗口
        /// </summary>
        public Window StartWindow
        {
            get { return new MetroWindow(); }
        }

        public List<string> ServiceReceiptMessageType
        {
            get { return new List<string>(); }
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


        public UserControl StartControl
        {
            get { return null; }
        }
    }
}
