using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.CmptRuntime
{
    public class ViewsBlockModel
    {
        [JsonProperty(PropertyName = "blockname")]
        private string blockName;
        /// <summary>
        /// Block名称
        /// </summary>
        public string BlockName
        {
            get
            {
                return blockName;
            }
            set
            {
                blockName = value;
            }
        }
        [JsonProperty(PropertyName = "datasettype")]
        private string datasetType;
        /// <summary>
        /// 数据集类型
        /// </summary>
        public string DatasetType
        {
            get
            {
                return datasetType;
            }
            set
            {
                datasetType = value;
            }
        }
        [JsonProperty(PropertyName = "superiors")]
        private string superiors;
        /// <summary>
        /// 上级Block的名称
        /// </summary>
        public string Superiors
        {
            get
            {
                return superiors;
            }
            set
            {
                superiors = value;
            }
        }
        [JsonProperty(PropertyName = "tablename")]
        private string tableName;
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            get
            {
                return tableName;
            }
            set
            {
                tableName = value;
            }
        }
        [JsonProperty(PropertyName = "conditions")]
        private object conditions;
        /// <summary>
        /// 条件(检索条件、排序及分页)
        /// </summary>
        public object Conditions
        {
            get
            {
                return conditions;
            }
            set
            {
                conditions = value;
            }
        }
    }
}
