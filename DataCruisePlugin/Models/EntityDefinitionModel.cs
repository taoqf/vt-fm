using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DataCruisePlugin.Models
{
    /// <summary>
    /// 实体定义类
    /// </summary>
    internal class EntityDefinitionModel
    {
        /// <summary>
        /// 实体主键
        /// </summary>
        [JsonProperty(PropertyName = "_id")]
        public string Id
        {
            get;
            set;
        }
        /// <summary>
        /// 表名
        /// </summary>
        [JsonProperty(PropertyName="tablename")]
        public string TableName
        {
            get;
            set;
        }
        /// <summary>
        /// Tab标题
        /// </summary>
        [JsonProperty(PropertyName="tabtitle")]
        public string TabTitle
        {
            get;
            set;
        }
        /// <summary>
        /// 是否为入口Tab
        /// </summary>
        [JsonProperty(PropertyName="entrance")]
        public string Entrance
        {
            get;
            set;
        }
        /// <summary>
        /// 显示类型
        /// </summary>
        [JsonProperty(PropertyName = "viewtype")]
        public string ViewType
        {
            get;
            set;
        }
        /// <summary>
        /// 数据引用
        /// </summary>
        [JsonProperty(PropertyName = "dataref")]
        public object DataRef
        {
            get;
            set;
        }
        /// <summary>
        /// 表字段结构
        /// </summary>
        [JsonProperty(PropertyName = "fields")]
        public List<EntityFieldModel> Fields
        {
            get;
            set;
        }
        /// <summary>
        /// 动态列
        /// </summary>
        [JsonProperty(PropertyName = "dynacolumn")]
        public List<string> DynaColumn
        {
            get;
            set;
        }
        /// <summary>
        /// 动态Tab
        /// </summary>
        [JsonProperty(PropertyName = "dynatab")]
        public string DynaTab
        {
            get;
            set;
        }
    }
}
