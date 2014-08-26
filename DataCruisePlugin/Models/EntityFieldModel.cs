using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DataCruisePlugin.Models
{
    /// <summary>
    /// 实体字段类
    /// </summary>
    internal class EntityFieldModel
    {
        /// <summary>
        /// 字段名
        /// </summary>
        [JsonProperty(PropertyName = "field")]
        public string Field
        {
            get;
            set;
        }
        /// <summary>
        /// 字段标题
        /// </summary>
        [JsonProperty(PropertyName = "fieldtitle")]
        public string FieldTitle
        {
            get;
            set;
        }
        /// <summary>
        /// 字段类型
        /// </summary>
        [JsonProperty(PropertyName = "fieldtype")]
        public string FieldType
        {
            get;
            set;
        }
    }
}
