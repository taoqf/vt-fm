﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Server.Controls.Runtime
{
    /// <summary>
    /// 定义插件实体
    /// </summary>
    public class DefinPluginsModel
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        [JsonProperty(PropertyName = "pluginName")]
        private string PluginName
        {
            get;
            set;
        }
        /// <summary>
        /// 插件展示类型
        /// </summary>
        [JsonProperty(PropertyName = "pluginType")]
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
    }
}
