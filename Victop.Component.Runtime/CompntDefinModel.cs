using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Component.Runtime
{
    /// <summary>
    /// 组件定义类
    /// </summary>
    public class CompntDefinModel
    {
        /// <summary>
        /// 视图集合
        /// </summary>
        [JsonProperty(PropertyName = "views")]
        public List<DefinViewsModel> Views
        {
            get;
            set;
        }
        /// <summary>
        /// 插件集合
        /// </summary>
        [JsonProperty(PropertyName = "widgets")]
        public List<DefinPluginsModel> Plugins
        {
            get;
            set;
        }
        /// <summary>
        /// 组件参数设置
        /// </summary>
        [JsonProperty(PropertyName = "compoSetting")]
        public List<object> CompoSetting
        {
            get;
            set;
        }
        /// <summary>
        /// 参数映射
        /// </summary>
        [JsonProperty(PropertyName = "paramMap")]
        public List<DefinParamMapModel> ParamMap
        {
            get;
            set;
        }
    }
}
