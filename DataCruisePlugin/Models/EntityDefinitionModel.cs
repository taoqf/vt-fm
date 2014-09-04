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
    public class EntityDefinitionModel:ICloneable
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
        public bool Entrance
        {
            get;
            set;
        }
        /// <summary>
        /// 主表
        /// </summary>
        [JsonProperty(PropertyName = "hosttable")]
        public string HostTable
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
        /// 父级标识
        /// </summary>
        [JsonProperty(PropertyName = "parentid")]
        public string ParentId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "treedisplay")]
        public string TreeDisPlay
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
        public object Fields
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
        public List<string> DynaTab
        {
            get;
            set;
        }
        /// <summary>
        /// 数据路径
        /// </summary>
        [JsonIgnore]
        public string DataPath
        {
            get;
            set;
        }
        /// <summary>
        /// 视图标识
        /// </summary>
        [JsonIgnore]
        public string ViewId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否激活
        /// </summary>
        [JsonIgnore]
        public bool Actived
        {
            get;
            set;
        }
        /// <summary>
        /// 引用信息
        /// </summary>
        [JsonIgnore]
        public RefEntityModel RefInfo
        {
            get;
            set;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
        /// <summary>
        /// 实体克隆
        /// </summary>
        /// <returns></returns>
        public EntityDefinitionModel Copy()
        {
            return this.Clone() as EntityDefinitionModel;
        }
    }
}
