using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Data;
using Victop.Frame.DataChannel;
using Victop.Frame.PublicLib.Helpers;

namespace Victop.Component.Runtime
{
    /// <summary>
    /// view块实体
    /// </summary>
    public class ViewsBlockModel:ICloneable
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
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public ViewsBlockModel Copy(string blockName)
        {
            this.BlockLock = true;
            ViewsBlockModel BlockModel= this.Clone() as ViewsBlockModel;
            BlockModel.BlockName=blockName;
            BlockModel.BlockDataPath.Clear();
            return BlockModel;
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
        /// <summary>
        /// 获取JsonData
        /// </summary>
        /// <returns></returns>
        public string GetJsonData()
        {
            if (string.IsNullOrEmpty(ViewId) || BlockDataPath == null || BlockDataPath.Count <= 0)
            {
                return string.Empty;
            }
            else
            {
                DataOperation dataOp = new DataOperation();
                return dataOp.GetJSONData(ViewId, JsonHelper.ToJson(BlockDataPath));
            }
        }
    }
}
