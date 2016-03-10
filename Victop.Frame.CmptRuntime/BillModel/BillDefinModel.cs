using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 单据实体类
    /// </summary>
    public class BillDefinModel : PropertyModelBase
    {        
        /// <summary>
        /// 单据状态分类
        /// </summary>
        private object gsdocstate;
        /// <summary>
        /// 单据状态分类
        /// </summary>
        [JsonProperty(PropertyName = "gsdocstate")]
        public object Gsdocstate
        {
            get { return gsdocstate; }
            set { gsdocstate = value; }
        }

        /// <summary>
        /// 编码服务
        /// </summary>
        private object encodedService;
        /// <summary>
        /// 编码服务
        /// </summary>
        [JsonProperty(PropertyName = "encoded_service")]
        public object EncodedService
        {
            get { return encodedService; }
            set { encodedService = value; }
        }

        /// <summary>
        /// 动态字段
        /// </summary>
        private object dynamicFields;
        /// <summary>
        /// 动态字段
        /// </summary>
        [JsonProperty(PropertyName = "dynamic_fields")]
        public object DynamicFields
        {
            get { return dynamicFields; }
            set { dynamicFields = value; }
        }
    }
}
