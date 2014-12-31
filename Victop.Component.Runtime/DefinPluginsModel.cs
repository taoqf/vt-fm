using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Component.Runtime
{
    /// <summary>
    /// 定义插件实体
    /// </summary>
    public class DefinPluginsModel
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        [JsonProperty(PropertyName = "widgetName")]
        public string PluginName
        {
            get;
            set;
        }
        /// <summary>
        /// 插件展示类型
        /// </summary>
        [JsonProperty(PropertyName = "widgetType")]
        public string PluginType
        {
            get;
            set;
        }
        /// <summary>
        /// 数据块名称
        /// </summary>
        [JsonProperty(PropertyName = "dataBlock")]
        public string DataBlock
        {
            get;
            set;
        }
        /// <summary>
        /// 参数集合
        /// </summary>
        [JsonProperty(PropertyName = "Params")]
        public List<object> Params
        {
            get;
            set;
        }
        /// <summary>
        /// 插件Block
        /// </summary>
        [JsonIgnore]
        public ViewsBlockModel PluginBlock
        {
            get;
            set;
        }
    }
}
