using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victop.Frame.DataMessageManager.Models
{
    /// <summary>
    /// 启动插件参数实体
    /// </summary>
    public class ExcutePluginParamModel
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        public string PluginName { get; set; }
        /// <summary>
        /// 插件目录
        /// </summary>
        public string PluginPath { get; set; }
        /// <summary>
        /// 展示标题
        /// </summary>
        public string ShowTitle { get; set; }
        /// <summary>
        /// 是否在活动列表中展示
        /// </summary>
        public bool VisiblePlugin { get; set; } = true;
        /// <summary>
        /// 是否强制加载
        /// </summary>
        public bool IsLoading { get; set; }
    }
}
