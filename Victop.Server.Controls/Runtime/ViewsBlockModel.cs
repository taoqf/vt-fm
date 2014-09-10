using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Data;

namespace Victop.Server.Controls.Runtime
{
    /// <summary>
    /// view块实体
    /// </summary>
    public class ViewsBlockModel
    {
        /// <summary>
        /// 块名称
        /// </summary>
        [JsonProperty(PropertyName = "blockName")]
        public string BlockName
        {
            get;
            set;
        }
        /// <summary>
        /// 块类型
        /// </summary>
        [JsonProperty(PropertyName = "blocktype")]
        public string BlockType
        {
            get;
            set;
        }
        /// <summary>
        /// 数据集类型
        /// </summary>
        [JsonProperty(PropertyName = "dataSetType")]
        public string DataSetType
        {
            get;
            set;
        }
        /// <summary>
        /// 上级Block的名称
        /// </summary>
        [JsonProperty(PropertyName = "superiors")]
        public string Superiors
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
        /// 表名称
        /// </summary>
        [JsonIgnore]
        public string TableName
        {
            get;
            set;
        }
        /// <summary>
        /// 块数据集
        /// </summary>
        [JsonIgnore]
        public DataTable BlockDt
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
    }
}
