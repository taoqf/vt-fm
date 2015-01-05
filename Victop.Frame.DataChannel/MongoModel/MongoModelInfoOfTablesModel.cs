using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.DataChannel.MongoModel
{
    /// <summary>
    /// Mongo模型定义中表集合实体
    /// </summary>
    public class MongoModelInfoOfTablesModel
    {
        /// <summary>
        /// 表名
        /// </summary>
        private string tableName;
        /// <summary>
        /// 表名
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }
        /// <summary>
        /// 查询时不包含的字段
        /// </summary>
        private List<string> tableExclude;
        /// <summary>
        /// 查询时不包含的字段
        /// </summary>
        [JsonProperty(PropertyName = "exclude")]
        public List<string> TableExclude
        {
            get
            {
                if (tableExclude == null)
                    tableExclude = new List<string>();
                return tableExclude;
            }
            set { tableExclude = value; }
        }
        /// <summary>
        /// 表结构
        /// </summary>
        private List<MongoModelInfoOfTableStructureModel> tableStructure;
        /// <summary>
        /// 表结构
        /// </summary>
        [JsonProperty(PropertyName = "structure")]
        public List<MongoModelInfoOfTableStructureModel> TableStructure
        {
            get
            {
                if (tableStructure == null)
                    tableStructure = new List<MongoModelInfoOfTableStructureModel>();
                return tableStructure;
            }
            set { tableStructure = value; }
        }
    }
}
