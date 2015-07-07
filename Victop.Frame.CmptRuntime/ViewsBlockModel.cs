using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Data;

namespace Victop.Frame.CmptRuntime
{
    public class ViewsBlockModel : ICloneable
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
        private ViewsBlockConditionModel conditions;
        /// <summary>
        /// 条件(检索条件、排序及分页)
        /// </summary>
        public ViewsBlockConditionModel Conditions
        {
            get
            {
                if (conditions == null)
                    conditions = new ViewsBlockConditionModel();
                return conditions;
            }
            set
            {
                conditions = value;
            }
        }
        /// <summary>
        /// 12-13新增
        /// </summary>
        public DefinViewsModel ViewModel
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
        /// 块数据集
        /// </summary>
        [JsonIgnore]
        public DataSet BlockDt
        {
            get;
            set;
        }
        /// <summary>
        /// 块路径
        /// </summary>
        [JsonIgnore]
        public List<object> BlockDataPath
        {
            get;
            set;
        }
        /// <summary>
        /// 当前选择行
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, object> CurrentRow
        {
            get;
            set;
        }
        /// <summary>
        /// 数据块锁
        /// </summary>
        [JsonIgnore]
        public bool BlockLock
        {
            get;
            set;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public ViewsBlockModel Copy(string blockName)
        {
            this.BlockLock = true;
            ViewsBlockModel BlockModel = this.Clone() as ViewsBlockModel;
            BlockModel.BlockName = blockName;
            BlockModel.BlockDataPath = new List<object>();
            return BlockModel;
        }
    }
}
