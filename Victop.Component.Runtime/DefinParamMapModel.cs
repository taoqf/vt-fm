﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Component.Runtime
{
    /// <summary>
    /// 组件定义-参数映射实体
    /// </summary>
    public class DefinParamMapModel
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
        /// 插件参数名称
        /// </summary>
        [JsonProperty(PropertyName = "paramName")]
        public string ParamName
        {
            get;
            set;
        }
        /// <summary>
        /// 组件参数名称
        /// </summary>
        [JsonProperty(PropertyName = "compoParam")]
        public string CompoParam
        {
            get;
            set;
        }
    }
}
