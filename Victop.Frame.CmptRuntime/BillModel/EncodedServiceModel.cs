using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace Victop.Frame.CmptRuntime
{
    public class EncodedServiceModel : PropertyModelBase
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        private string paramName;
        /// <summary>
        /// 参数名称
        /// </summary>
        [JsonProperty(PropertyName = "param_name")]
        public string ParamName
        {
            get { return paramName; }
            set { paramName = value; }
        }

        /// <summary>
        /// 参数序号
        /// </summary>
        private string paramOrder;
        /// <summary>
        /// 参数序号
        /// </summary>
        [JsonProperty(PropertyName = "param_order")]
        public string ParamOrder
        {
            get { return paramOrder; }
            set { paramOrder = value; }
        }

        /// <summary>
        /// 参数类型
        /// </summary>
        private string paramType;
        /// <summary>
        /// 参数类型
        /// </summary>
        [JsonProperty(PropertyName = "param_type")]
        public string ParamType
        {
            get { return paramType; }
            set { paramType = value; }
        }

        /// <summary>
        /// 参数值
        /// </summary>
        private string paramValue;
        /// <summary>
        /// 参数值
        /// </summary>
        [JsonProperty(PropertyName = "param_value")]
        public string ParamValue
        {
            get { return paramValue; }
            set { paramValue = value; }
        }
    }
}
