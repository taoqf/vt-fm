using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Server.Controls.Runtime
{
    /// <summary>
    /// 组件定义
    /// </summary>
    public class ComponentModel
    {
        /// <summary>
        /// 组件名称
        /// </summary>
        [JsonProperty(PropertyName = "componentName")]
        public string ComponentName
        {
            get;
            set;
        }
        /// <summary>
        /// 组件类型
        /// </summary>
        [JsonProperty(PropertyName = "compoType")]
        public string CompoType
        {
            get;
            set;
        }
        /// <summary>
        /// 组件编号
        /// </summary>
        [JsonProperty(PropertyName = "component_No")]
        public string ComponentNo
        {
            get;
            set;
        }
        /// <summary>
        /// 组件定义
        /// </summary>
        [JsonProperty(PropertyName = "compntDefin")]
        public CompntDefinModel CompntDefin
        {
            get;
            set;
        }
    }
}
