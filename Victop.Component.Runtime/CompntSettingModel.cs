using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Component.Runtime
{
    /// <summary>
    /// 组件配置实体
    /// </summary>
    public class CompntSettingModel
    {
        /// <summary>
        /// 组件配置信息
        /// </summary>
        [JsonProperty(PropertyName = "compoSetting")]
        public Dictionary<string,object> CompoSetting
        {
            get;
            set;
        }
        /// <summary>
        /// 插件配置信息
        /// </summary>
        [JsonProperty(PropertyName = "pluginsSetting")]
        public Dictionary<string, object> PluginsSetting
        {
            get;
            set;
        }
        /// <summary>
        /// 视图配置信息
        /// </summary>
        [JsonProperty(PropertyName = "viewSetting")]
        public Dictionary<string, object> ViewSetting
        {
            get;
            set;
        }
    }
}
