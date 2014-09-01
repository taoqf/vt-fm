using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Victop.Server.Controls;
using DataCruisePlugin.Views;
using System.Windows.Controls;

namespace DataCruisePlugin
{
    public class Plugin:IPlugin
    {
        /// <summary>
        /// 插件标题
        /// </summary>
        public string PluginTitle
        {
            get { return "数据巡航插件"; }
        }
        /// <summary>
        /// 插件名称
        /// </summary>
        public string PluginName
        {
            get { return "DataCruisePlugin"; }
        }
        /// <summary>
        /// 消息类型
        /// </summary>
        public List<string> ServiceReceiptMessageType
        {
            get { return new List<string>() ; }
        }
        /// <summary>
        /// 显示方式
        /// </summary>
        public int ShowType
        {
            get { return 1; }
        }
        /// <summary>
        /// 是否为系统插件
        /// </summary>
        public int SystemPlugin
        {
            get { return 0; }
        }
        /// <summary>
        /// 是否自动加载
        /// </summary>
        public int AutoInit
        {
            get { return 0; }
        }
        /// <summary>
        /// 启动窗口
        /// </summary>
        public Window StartWindow
        {
            get { return new CruiseWindow(); }
        }
        /// <summary>
        /// 启动用户控件
        /// </summary>
        public UserControl StartControl
        {
            get { return new UCCruiseWindowNew(); }
        }
        /// <summary>
        /// 初始化方法
        /// </summary>
        public void Init()
        {
            //
        }
        /// <summary>
        /// 参数键值对
        /// </summary>
        public Dictionary<string, object> ParamDict
        {
            get
            {
                if (ShowType == 0)
                {
                    return CruiseWindow.ParamDict;
                }
                else
                {
                    return UCCruiseWindowNew.ParamDict;
                }
            }
            set
            {
                UCCruiseWindow.ParamDict = value;
                UCCruiseWindowNew.ParamDict = value;
            }
        }
    }
}
