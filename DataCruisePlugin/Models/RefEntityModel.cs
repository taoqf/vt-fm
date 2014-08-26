using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DataCruisePlugin.Models
{
    /// <summary>
    /// 引用实体定义
    /// </summary>
    internal class RefEntityModel
    {
        /// <summary>
        /// 表标识
        /// </summary>
        [JsonProperty(PropertyName = "table_id")]
        public string TableId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否为前导表
        /// </summary>
        [JsonProperty(PropertyName = "forerunner")]
        public bool ForeRunner
        {
            get;
            set;
        }
       /// <summary>
       /// 源字段
       /// </summary>
        [JsonProperty(PropertyName = "sourcefield")]
        public string SourceField
        {
            get;
            set;
        }
        /// <summary>
        /// 自有字段
        /// </summary>
        [JsonProperty(PropertyName = "selffield")]
        public string SelfField
        {
            get;
            set;
        }
    }
}
